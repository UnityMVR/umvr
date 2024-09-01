using System;
using Model.TestModel;
using Model.TestModel.Generated;
using NUnit.Framework;
using pindwin.umvr.Serialization.BytePatch;
using Tests.Utilities;
using UnityEngine;
using Zenject;

namespace Model
{
	public class Model_ByteSerialization_IntegrationTests
	{
		private DiContainer _container;
		private TestsInstaller _installer;

		private ITestModel _testModel;
		private TestModelFactory _factory;
		private BytePatchSerializer<TestModelModel> _testModelSerializer;
		private byte[] _buffer;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			TestsInstaller.SetUp(out _container, out _installer, InstallBindings);
			
			void InstallBindings(DiContainer container)
			{
				TestModelInstallerBase.Install(container);
				_container.BindInterfacesAndSelfTo<BytePatchSerializer<TestModelModel>>().AsSingle();
			}
		}
		
		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			TestsInstaller.TearDown(ref _container, ref _installer);
		}

		[SetUp]
		public void SetUp()
		{
			_buffer = new byte[BytePatchSerializationPayload.PATCH_SIZE_DEFAULT];
			_factory = _container.Resolve<TestModelFactory>();
			_testModel = _factory.Create(10, DateTime.Now.ToShortDateString(), 17);
			_testModelSerializer = _container.Resolve<BytePatchSerializer<TestModelModel>>();
		}

		[TearDown]
		public void TearDown()
		{
			_testModel.Dispose();
		}

		[Test]
		public void BytePatchSerializer_Serialize_returns_non_empty_result()
		{
			using BytePatchSerializationPayload payload = new BytePatchSerializationPayload();
			_testModelSerializer.Serialize((TestModelModel)_testModel, payload);
			Assert.IsTrue(payload != null && payload.Position > 0);
		}
		
		[Test]
		public void BytePatchSerializer_Serialize_returns_result_that_can_be_Deserialized_back()
		{
			_testModel.FloatField = Mathf.PI;
			_testModel.StubbedStringField = DateTime.Now.ToLongDateString();
			BytePatchSerializationPayload writingPayload = new BytePatchSerializationPayload(_buffer);
			_testModelSerializer.Serialize((TestModelModel)_testModel, writingPayload);
			
			TestModelMemo memo = new TestModelMemo(_testModel);
			_testModel.Dispose();

			ITestModel deserialized;
			using (BytePatchDeserializationPayload readingPayload = new BytePatchDeserializationPayload(_buffer))
			{
				deserialized = _factory.Create(readingPayload);
			}
			
			Assert.AreEqual(memo.Id, deserialized.Id);
			Assert.AreEqual(memo.IntField, deserialized.IntField);
			Assert.AreEqual(memo.FloatField, deserialized.FloatField);
			Assert.AreEqual(memo.ExplicitlyInitializedStringField, deserialized.ExplicitlyInitializedStringField);
			Assert.AreEqual(memo.StubbedStringField, deserialized.StubbedStringField);
		}
	}
}
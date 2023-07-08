using System;
using System.Globalization;
using Model.TestModel;
using Model.TestModel.Generated;
using NUnit.Framework;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Serialization;
using Tests.Utilities;
using Zenject;
using Random = UnityEngine.Random;

namespace Model
{
	public class ModelFactory_Tests
	{
		private DiContainer _container;
		private TestsInstaller _installer;

		private TestModelFactory _factory;
		private ITestModel _model;
		
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			TestsInstaller.SetUp(out _container, out _installer, InstallBindings);
			
			void InstallBindings(DiContainer container)
			{
				TestModelInstallerBase.Install(container);
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
			_factory = _container.Resolve<TestModelFactory>();
		}

		[TearDown]
		public void TearDown()
		{
			_model?.Dispose();
		}

		[Test]
		public void ModelFactory_Create_instantiates_objects_with_correct_values()
		{
			int param0 = Random.Range(0, int.MaxValue);
			string param1 = DateTime.Now.ToString(CultureInfo.InvariantCulture);
			int param2 = 0;
			
			_model = _factory.Create(param0, param1, param2);
			
			Assert.AreEqual(param0, _model.IntField);
			Assert.AreEqual(param1, _model.ExplicitlyInitializedStringField);
		}
		
		[Test]
		public void ModelFactory_Create_TArg_called_with_invalid_serialization_params_throws_exception()
		{
			Assert.Throws<UMVRException>(() =>
			{
				_factory.Create(new NotInstalledSerializationParameter());
			});
		}
		
		[Test]
		public void ModelFactory_Create_TArg_called_with_null_serialization_params_does_not_throw_exception()
		{
			Assert.DoesNotThrow(() =>
			{
				_factory.Create<NullDeserializationParam>(null);
			});
		}

		private class NotInstalledSerializationParameter { }
	}
}
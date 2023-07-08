using System;
using Model.TestCollection;
using Model.TestCollection.Generated;
using Model.TestModel;
using Model.TestModel.Generated;
using NUnit.Framework;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using Tests.Utilities;
using Zenject;
using UniRx;

namespace Model
{
	public class Model_Dispose_IntegrationTests
	{
		private DiContainer _container;
		private TestsInstaller _installer;

		private TestModelModel _testModel;
		private ITestCollection _testCollection;
		
		private TestModelFactory _factory;
		private TestModelRepository _repository;

		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			TestsInstaller.SetUp(out _container, out _installer, InstallBindings);
			
			void InstallBindings(DiContainer container)
			{
				TestModelInstallerBase.Install(container);
				TestCollectionInstallerBase.Install(container);
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
			_repository = _container.Resolve<TestModelRepository>();
			_testModel = (TestModelModel)_factory.Create(default, default, default);
			_testCollection = _container.Resolve<TestCollectionFactory>().Create();
			_testCollection.Collection.Add(_testModel);
		}

		[TearDown]
		public void TearDown()
		{
			_testModel?.Dispose();
			_testCollection?.Dispose();
		}

		[Test]
		public void Model_Dispose_causes_Model_Disposing_to_invoke_exactly_once()
		{
			int disposingCount = 0;
			_testModel.Disposing += _ => SetFlag();
			_testModel.Dispose();
			_testModel.Dispose();
			Assert.True(disposingCount == 1);

			void SetFlag()
			{
				disposingCount += 1;
			}
		}

		[Test]
		public void Model_Dispose_invokes_Model_Disposing_then_removes_it_from_Collection()
		{
			AssertCollectionCount(1);
			_testModel.Disposing += _ => AssertCollectionCount(1);
			_testModel.Dispose();
			AssertCollectionCount(0);
		}

		[Test]
		public void Model_Dispose_removes_model_from_Collection_then_removes_it_from_Repository()
		{
			IDisposable subscription = default;
			try
			{
				AssertRepositoryCount(1);
				AssertCollectionCount(1);

				subscription = _testCollection.GetCollection<ITestModel>(nameof(ITestCollection.Collection))
					.ObserveRemove().Subscribe(_ =>
					{
						AssertCollectionCount(0);
						AssertRepositoryCount(1);
					});
				
				_testModel.Dispose();
				
				AssertRepositoryCount(0);
				AssertCollectionCount(0);
			}
			finally
			{
				subscription?.Dispose();
			}
		}

		[Test]
		public void Model_Dispose_removes_Model_from_repository_then_invalidates_Model()
		{
			AssertModelValid(true);
			AssertRepositoryCount(1);

			_repository.Removed += m =>
			{
				AssertModelValid(true);
				AssertRepositoryCount(0);
			};
			
			_testModel.Dispose();
			
			AssertModelValid(false);
		}

		private void AssertRepositoryCount(int count)
		{
			Assert.True(_repository.Entities.Count == count);
		}
		
		private void AssertCollectionCount(int count)
		{
			Assert.True(_testCollection.Collection.Count == count);
		}

		void AssertModelValid(bool isValid)
		{
			Assert.AreEqual(_testModel.Id != Id.UNKNOWN, isValid);
			foreach (string label in _testModel)
			{
				if (isValid)
				{
					Assert.DoesNotThrow(() => _testModel.GetProperty(label));
				}
				else
				{
					Assert.Throws<UMVRException>(() => _testModel.GetProperty(label));
				}
			}
		}
	}
}
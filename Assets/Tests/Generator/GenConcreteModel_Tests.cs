using System;
using System.Linq;
using GenerationParams;
using NUnit.Framework;
using pindwin.development;
using pindwin.umvr.Attributes;

namespace Tests.Generator
{
	public class GenConcreteModel_Tests
	{
		private const string Namespace = nameof(Namespace);
		private GenConcreteModel _collectionsModel;
		private GenConcreteModel _propertiesModel;

		[SetUp]
		public void SetUp()
		{
			_propertiesModel = new GenConcreteModel(Namespace, typeof(IGenTest), new NullLogger());
			_collectionsModel = new GenConcreteModel(Namespace, typeof(ICollGenTest), new NullLogger());
		}

		[TestCase(nameof(ICollGenTest.SimpleProperty), false, false, false, InitializationLevel.Default)]
		[TestCase(nameof(ICollGenTest.CollectionProperty), true, false, false, InitializationLevel.Default)]
		[TestCase(nameof(ICollGenTest.ReadonlyCollectionProperty), true, true, false, InitializationLevel.Explicit)]
		[TestCase(nameof(ICollGenTest.ModelCollectionProperty), true, false, true, InitializationLevel.Default)]
		[TestCase(nameof(ICollGenTest.ReadonlyModelCollectionProperty), true, true, true, InitializationLevel.Explicit)]
		public void GenConcreteModel_collection_Properties_have_proper_flags(
			string propertyName,
			bool isCollection,
			bool isReadonly,
			bool isModel,
			InitializationLevel initializationLevel)
		{
			Property property = _collectionsModel.Properties.FirstOrDefault(p => p.Name == propertyName);
			Assert.NotNull(property);
			Assert.True(property.IsCollection == isCollection);
			Assert.True(property.IsReadonly == isReadonly);
			Assert.True(property.IsModel == isModel);
			Assert.True(property.InitializationLevel == initializationLevel);
		}

		[TestCase(nameof(IGenTest.SimpleProperty), typeof(int), ExpectedResult = false)]
		[TestCase(nameof(IGenTest.SimpleModelProperty), typeof(IGenTest), ExpectedResult = false)]
		[TestCase(nameof(IGenTest.ReadonlyProperty), typeof(int), ExpectedResult = true)]
		[TestCase(nameof(IGenTest.ReadonlyModelProperty), typeof(IGenTest), ExpectedResult = true)]
		[TestCase(nameof(IGenTest.CustomProperty), typeof(int), ExpectedResult = false)]
		[TestCase(nameof(IGenTest.ProperlySkippedProperty), typeof(int), ExpectedResult = false)]
		[TestCase(nameof(IGenTest.InitializedDespiteAttributeProperty), typeof(int), ExpectedResult = true)]
		[TestCase(nameof(IGenTest.ExplicitlyInitializedProperty), typeof(int), ExpectedResult = true)]
		[TestCase("param0", typeof(float), ExpectedResult = true)]
		[TestCase("param1", typeof(int), ExpectedResult = false)]
		public bool GenConcreteModel_constructor_has_proper_parameters(string parameterName, Type type)
		{
			Assert.True(_propertiesModel.Constructors.Count == 1);
			return _propertiesModel.Constructors[0].Params
				.Any(p => p.Name.ToLower() == parameterName.ToLower() && p.Type == type.FullName);
		}

		[Test]
		public void GenConcreteModel_has_correct_name_type_and_namespace()
		{
			Assert.True(_propertiesModel.Name == "GenTest");
			Assert.True(_propertiesModel.Type == "GenTestModel");
			Assert.True(_propertiesModel.Namespace == Namespace);
			
			Assert.True(_collectionsModel.Name == "CollGenTest");
			Assert.True(_collectionsModel.Type == "CollGenTestModel");
			Assert.True(_collectionsModel.Namespace == Namespace);
		}

		[TestCase(nameof(IGenTest.SimpleProperty), false, false, false, InitializationLevel.Default)]
		[TestCase(nameof(IGenTest.SimpleModelProperty), false, true, false, InitializationLevel.Default)]
		[TestCase(nameof(IGenTest.ReadonlyProperty), true, false, false, InitializationLevel.Explicit)]
		[TestCase(nameof(IGenTest.ReadonlyModelProperty), true, true, false, InitializationLevel.Explicit)]
		[TestCase(nameof(IGenTest.CustomProperty), false, false, true, InitializationLevel.Default)]
		[TestCase(nameof(IGenTest.ProperlySkippedProperty), false, false, false, InitializationLevel.Skip)]
		[TestCase(nameof(IGenTest.InitializedDespiteAttributeProperty), true, false, false, InitializationLevel.Explicit)]
		[TestCase(nameof(IGenTest.ExplicitlyInitializedProperty), false, false, false, InitializationLevel.Explicit)]
		public void GenConcreteModel_Properties_have_proper_flags(
			string propertyName,
			bool isReadonly,
			bool isModel,
			bool stub,
			InitializationLevel initializationLevel)
		{
			Property p = _propertiesModel.Properties.FirstOrDefault(p => p.Name == propertyName);
			Assert.NotNull(p);
			Assert.True(isReadonly == p.IsReadonly);
			Assert.True(isModel == p.IsModel);
			Assert.True(stub == p.CustomImplementation);
			Assert.True(initializationLevel == p.InitializationLevel);
		}
	}
}
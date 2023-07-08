using System.Collections.Generic;
using pindwin.umvr.Attributes;
using pindwin.umvr.Model;

namespace Tests.Generator
{
	[AdditionalParameters(typeof(float))]
	internal interface IGenTest : IModel
	{
		int SimpleProperty { get; set; }
		IGenTest SimpleModelProperty { get; set; }
		int ReadonlyProperty { get; }
		IGenTest ReadonlyModelProperty { get; }
		[CustomImplementation] int CustomProperty { get; set; }
		[Initialization(InitializationLevel.Skip)] int ProperlySkippedProperty { get; set; }
		[Initialization(InitializationLevel.Skip)] int InitializedDespiteAttributeProperty { get; }
		[Initialization(InitializationLevel.Explicit)] int ExplicitlyInitializedProperty { get; set; }
	}

	internal interface ICollGenTest : IModel
	{
		int SimpleProperty { get; set; }
		IList<int> CollectionProperty { get; set; }
		IList<int> ReadonlyCollectionProperty { get; }
		IList<IGenTest> ModelCollectionProperty { get; set; }
		IList<IGenTest> ReadonlyModelCollectionProperty { get; }
	}
}
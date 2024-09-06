using pindwin.umvr.Attributes;
using pindwin.umvr.example;
using pindwin.umvr.Model;
// ReSharper disable RedundantArgumentDefaultValue

namespace GenTest
{
	public interface ISingleGenTest : IModel
	{
		[Initialization(InitializationLevel.Default)] int Prop0 { get; set; }
		[Initialization(InitializationLevel.Explicit)] int Prop1 { get; set; }
		[Initialization(InitializationLevel.Skip)] int Prop2 { get; set; }
	
		[CustomImplementation, Initialization(InitializationLevel.Default)] int Prop3 { get; set; }
		[CustomImplementation, Initialization(InitializationLevel.Explicit)] int Prop4 { get; set; }
		[CustomImplementation, Initialization(InitializationLevel.Skip)] int Prop5 { get; set; }
	
		[Initialization(InitializationLevel.Default)] IFoo Prop6 { get; set; }
		[Initialization(InitializationLevel.Explicit)] IFoo Prop7 { get; set; }
		[Initialization(InitializationLevel.Skip)] IFoo Prop8 { get; set; }
		
		[CustomImplementation, Initialization(InitializationLevel.Default)] IFoo Prop9 { get; set; }
		[CustomImplementation, Initialization(InitializationLevel.Explicit)] IFoo Prop10 { get; set; }
		[CustomImplementation, Initialization(InitializationLevel.Skip)] IFoo Prop11 { get; set; }
	
	}
}

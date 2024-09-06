using pindwin.umvr.Attributes;
using pindwin.umvr.example;
using pindwin.umvr.Model;
// ReSharper disable RedundantArgumentDefaultValue

namespace GenTest
{
	public interface IReadonlySingleGenTest : IModel
	{
		[Initialization(InitializationLevel.Default)] int Prop0 { get; }
		[Initialization(InitializationLevel.Explicit)] int Prop1 { get; }
		[Initialization(InitializationLevel.Skip)] int Prop2 { get; }
	
		[CustomImplementation, Initialization(InitializationLevel.Default)] int Prop3 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Explicit)] int Prop4 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Skip)] int Prop5 { get; }
		
		[Initialization(InitializationLevel.Default)] IFoo Prop7 { get; }
		[Initialization(InitializationLevel.Explicit)] IFoo Prop8 { get; }
		[Initialization(InitializationLevel.Skip)] IFoo Prop9 { get; }
	
		[CustomImplementation, Initialization(InitializationLevel.Default)] IFoo Prop10 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Explicit)] IFoo Prop11 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Skip)] IFoo Prop12 { get; }
	
	}
}

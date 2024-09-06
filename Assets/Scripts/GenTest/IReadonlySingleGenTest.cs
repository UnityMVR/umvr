using pindwin.umvr.Attributes;
using pindwin.umvr.example;
using pindwin.umvr.Model;
// ReSharper disable RedundantArgumentDefaultValue

namespace GenTest
{
	public interface IReadonlySingleGenTest : IModel
	{
		[Initialization(InitializationLevel.Default)] int Prop12 { get; }
		[Initialization(InitializationLevel.Explicit)] int Prop13 { get; }
		[Initialization(InitializationLevel.Skip)] int Prop14 { get; }
	
		[Initialization(InitializationLevel.Default)] IFoo Prop15 { get; }
		[Initialization(InitializationLevel.Explicit)] IFoo Prop16 { get; }
		[Initialization(InitializationLevel.Skip)] IFoo Prop17 { get; }
	
	}
}

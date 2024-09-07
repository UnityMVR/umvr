using System.Collections.Generic;
using pindwin.umvr.Attributes;
using pindwin.umvr.example;
using pindwin.umvr.Model;
// ReSharper disable RedundantArgumentDefaultValue

namespace GenTest
{
	public interface ICollectionGenTest : IModel
	{
		[Initialization(InitializationLevel.Default)] IList<int> Col0 { get; }
		[Initialization(InitializationLevel.Explicit)] IList<int> Col1 { get; }
		[Initialization(InitializationLevel.Skip)] IList<int> Col2 { get; }
		
		[CustomImplementation, Initialization(InitializationLevel.Default)] IList<int> Col3 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Explicit)] IList<int> Col4 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Skip)] IList<int> Col5 { get; }
		
		[Initialization(InitializationLevel.Default)] IList<IFoo> Col6 { get; }
		[Initialization(InitializationLevel.Explicit)] IList<IFoo> Col7 { get; }
		[Initialization(InitializationLevel.Skip)] IList<IFoo> Col8 { get; }
		
		[CustomImplementation, Initialization(InitializationLevel.Default)] IList<IFoo> Col9 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Explicit)] IList<IFoo> Col10 { get; }
		[CustomImplementation, Initialization(InitializationLevel.Skip)] IList<IFoo> Col11 { get; }
	}
}
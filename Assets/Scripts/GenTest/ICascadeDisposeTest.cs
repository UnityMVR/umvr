using System.Collections.Generic;
using pindwin.umvr.Attributes;
using pindwin.umvr.example;
using pindwin.umvr.Model;

namespace GenTest
{
	public interface ICascadeDisposeTest : IModel
	{
		[CascadeDispose(CascadeDirection.Both)] int Prop0 { get; set; }
		[CascadeDispose(CascadeDirection.Both)] int Prop1 { get; }
		
		[CascadeDispose(CascadeDirection.Both)] IFoo Prop2 { get; set; }
		[CascadeDispose(CascadeDirection.Both)] IFoo Prop3 { get; }
		
		[CascadeDispose(CascadeDirection.Both)] IList<int> Prop4 { get; set; }
		
		[CascadeDispose(CascadeDirection.Both)] IList<IFoo> Prop5 { get; set; }
		
		
		[CascadeDispose(CascadeDirection.Both), CustomImplementation] int Prop6 { get; set; }
		[CascadeDispose(CascadeDirection.Both), CustomImplementation] int Prop7 { get; }
		
		[CascadeDispose(CascadeDirection.Both), CustomImplementation] IFoo Prop8 { get; set; }
		[CascadeDispose(CascadeDirection.Both), CustomImplementation] IFoo Prop9 { get; }
		
		[CascadeDispose(CascadeDirection.Both), CustomImplementation] IList<int> Prop10 { get; set; }
		
		[CascadeDispose(CascadeDirection.Both), CustomImplementation] IList<IFoo> Prop11 { get; set; }
	}
}
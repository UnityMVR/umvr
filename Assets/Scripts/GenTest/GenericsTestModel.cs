using System.Collections.Generic;
using pindwin.umvr.Model;

namespace GenTest
{
	public partial class GenericsTestModel
	{
		public System.Nullable<System.Int32> NullableInt { get; set; }
		public System.Collections.Generic.HashSet<pindwin.umvr.example.IFoo> SingleParam { get; set; }
		public System.Collections.Generic.Dictionary<System.Int32,pindwin.umvr.example.IFoo> MultipleParams { get; set; }
		public System.Collections.Generic.IDictionary<System.Int32,pindwin.umvr.example.IFoo> MultipleParamsInterface { get; set; }
	}
}
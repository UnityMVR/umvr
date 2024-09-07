using System.Collections.Generic;
using pindwin.umvr.Model;

namespace GenTest
{
	public partial class CascadeDisposeTestModel
	{
		public System.Int32 Prop6 { get; set; }
		public System.Int32 Prop7 { get; private set; }
		public pindwin.umvr.example.IFoo Prop8 { get; set; }
		public pindwin.umvr.example.IFoo Prop9 { get; private set; }
		public IList<System.Int32> Prop10 { get; set; }
		public IList<pindwin.umvr.example.IFoo> Prop11 { get; set; }
	}
}
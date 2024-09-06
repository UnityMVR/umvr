using System.Collections.Generic;
using pindwin.umvr.Model;

namespace GenTest
{
	public partial class ReadonlySingleGenTestModel
	{
		public System.Int32 Prop3 { get; private set; }
		public System.Int32 Prop4 { get; private set; }
		public System.Int32 Prop5 { get; private set; }
		public pindwin.umvr.example.IFoo Prop10 { get; private set; }
		public pindwin.umvr.example.IFoo Prop11 { get; private set; }
		public pindwin.umvr.example.IFoo Prop12 { get; private set; }
	}
}
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace GenTest
{
	public partial class CollectionGenTestModel
	{
		public IList<System.Int32> Col3 { get; set; }
		public IList<System.Int32> Col4 { get; set; }
		public IList<System.Int32> Col5 { get; set; }
		public IList<pindwin.umvr.example.IFoo> Col9 { get; set; }
		public IList<pindwin.umvr.example.IFoo> Col10 { get; set; }
		public IList<pindwin.umvr.example.IFoo> Col11 { get; set; }
	}
}
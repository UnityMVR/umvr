using System.Collections.Generic;
using pindwin.umvr.Model;

namespace GenTest
{
	public partial class NonPropertyMembersTestModel
	{
		public System.Int32 Method0()
		{
			throw new System.NotImplementedException();
		}

		public System.Int32 Method1(System.Int32 param)
		{
			throw new System.NotImplementedException();
		}

		public void Method3<TParam>(TParam param)
		{
			throw new System.NotImplementedException();
		}

		public System.Collections.Generic.List<TParam> Method4<TParam>(System.Collections.Generic.HashSet<TParam> param)
		{
			throw new System.NotImplementedException();
		}

		public event System.Action Event0;
		public event System.Action<System.Int32> Event1;
		public System.Int32 this[System.Int32 index] { get => throw new System.NotImplementedException();  set => throw new System.NotImplementedException(); }
	}
}
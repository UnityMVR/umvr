using System;
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace GenTest
{
	public interface INonPropertyMembersTest : IModel
	{
		const int CONST0 = 0;
		
		static void StaticMethod0() { }
		
		int this[int index] { get; set; }
		
		int Method0();
		int Method1(int param);
		
		void Method3<TParam>(TParam param);
		List<TParam> Method4<TParam>(HashSet<TParam> param);

		event Action Event0;
		event Action<int> Event1;
	}
}
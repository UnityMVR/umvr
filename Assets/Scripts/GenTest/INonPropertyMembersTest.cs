using System;
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

		event Action Event0;
		event Action<int> Event1;
	}
}
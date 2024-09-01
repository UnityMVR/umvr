using System;

namespace pindwin.umvr.View.Widgets
{
	public interface IPropertyWidget
	{
		event Action<string> ValueChanged;
		string Label { get; set; }
		bool IsReadonly { get; set; }
	}
}
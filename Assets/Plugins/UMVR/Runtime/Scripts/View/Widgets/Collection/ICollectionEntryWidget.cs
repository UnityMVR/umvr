using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Widgets
{
	public interface ICollectionEntryWidget : IPropertyWidget, IValueContainer<string>
	{
		event Action<int> DeleteRequested; 
		int Index { get; set; }
	}
}
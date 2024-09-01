using System.Collections.Generic;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public sealed class DropdownsCollectionWidget : CollectionWidget
	{
		private readonly List<string> _choices;

		public DropdownsCollectionWidget(List<string> choices)
		{
			_choices = choices;
		}

		protected override ICollectionEntryWidget MakeEntry(int index, string value, VisualElement root)
		{
			var newEntry = new DropdownCollectionEntryWidget(index, _choices)
			{
				IsReadonly = IsReadonly
			};
			root.Insert(index, newEntry);
			return newEntry;
		}
	}
}
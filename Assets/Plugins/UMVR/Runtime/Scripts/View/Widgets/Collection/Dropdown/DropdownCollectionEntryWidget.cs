using System;
using System.Collections.Generic;
using pindwin.umvr.View.Binding;

namespace pindwin.umvr.View.Widgets
{
	public sealed class DropdownCollectionEntryWidget : DropdownWidget, ICollectionEntryWidget
	{
		private int _index;

		public DropdownCollectionEntryWidget(int index, List<string> choices)
			: this(index, string.Empty, choices) { }

		public DropdownCollectionEntryWidget(int index, string value, List<string> choices)
			: base(choices, $"[{index}]", value)
		{
			Index = index;
			MakeButton(NotifyDeleteRequested, DELETE_BUTTON, this);
			AddToClassList(COLLECTION_ENTRY_CLASS);
		}

		public event Action<int> DeleteRequested;

		public int Index
		{
			get => _index;
			set
			{
				_index = value;
				Label = $"[{_index}]";
			}
		}

		protected override string GetChangedValueImpl()
		{
			CollectionEvent ev = CollectionEvent.Replace(_index, DropdownField.value);
			return ev.ToString();
		}

		private void NotifyDeleteRequested()
		{
			DeleteRequested?.Invoke(Index);
		}
	}
}
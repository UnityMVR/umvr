using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public sealed class TextFieldsCollectionWidget : CollectionWidget
	{
		private TextFieldsCollectionEntryWidget _entry;

		protected override void CleanupEntry()
		{
			_entry.ValueProposed -= NotifyValueProposed;
		}

		protected override ICollectionEntryWidget MakeEntry(int index, string value, VisualElement root)
		{
			_entry = new TextFieldsCollectionEntryWidget(index, value)
			{
				IsReadonly = IsReadonly
			};
			_entry.ValueProposed += NotifyValueProposed;
			root.Insert(index, _entry);
			return _entry;
		}
	}
}
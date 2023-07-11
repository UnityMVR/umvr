using System;
using pindwin.umvr.View.Binding;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public sealed class TextFieldsCollectionEntryWidget : TextFieldWidget, ICollectionEntryWidget
	{
		private int _index;

		public TextFieldsCollectionEntryWidget(int index)
			: this(index, string.Empty) { }

		public TextFieldsCollectionEntryWidget(int index, string value)
			: base(string.Empty, value)
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
				FieldValue.label = $"[{_index}]";
			}
		}

		protected override string GetNotificationValueImpl()
		{
			CollectionEvent collectionEvent = CollectionEvent.Replace(Index, TempValue);
			return collectionEvent.ToString();
		}

		private void NotifyDeleteRequested()
		{
			DeleteRequested?.Invoke(Index);
		}
	}
}
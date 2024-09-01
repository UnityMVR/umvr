using System;
using System.Collections.Generic;
using pindwin.development;
using pindwin.umvr.View.Binding;
using UnityEngine.UIElements;

namespace pindwin.umvr.View.Widgets
{
	public abstract class CollectionWidget : DebuggerElement, IPropertyWidget
	{
		private readonly List<ICollectionEntryWidget> _entries;
		private readonly Foldout _foldout;
		private readonly Label _label;

		private bool _isReadonly;

		protected CollectionWidget()
		{
			_entries = new List<ICollectionEntryWidget>();

			style.flexDirection = FlexDirection.Column;
			AddToClassList(PROPERTY_CLASS);
			AddToClassList(COLLECTION_CLASS);

			VisualElement headerRoot = MakeHorizontalBar(this);
			_label = MakeLabel(string.Empty, headerRoot);
			MakeButton(OnClearRequested, CLEAR_BUTTON, headerRoot);
			MakeButton(OnAddRequested, ADD_BUTTON, headerRoot);
			_foldout = MakeFoldout(string.Empty, true, this);

			RefreshLabel();
		}

		public event Action<string> ValueChanged;
		public event Action<string> ValueProposed;

		public bool IsReadonly
		{
			get => _isReadonly;
			set
			{
				_isReadonly = value;
				foreach (ICollectionEntryWidget entry in _entries)
				{
					entry.IsReadonly = _isReadonly;
				}
			}
		}

		public string Label
		{
			get => _label.text;
			set
			{
				_label.text = value;
				MarkDirtyRepaint();
			}
		}

		public void Add(int index, string value)
		{
			index = index > 0 && index < _entries.Count ? index : _entries.Count;
			ICollectionEntryWidget widget = MakeEntry(index, value, _foldout);
			widget.ValueChanged += OnItemReplaced;
			widget.DeleteRequested += OnDeleteRequested;
			_entries.Insert(index, widget);
			for (int i = index + 1; i < _entries.Count; i++)
			{
				_entries[i].Index += 1;
			}

			RefreshLabel();
			MarkDirtyRepaint();
		}

		public void ClearCollection()
		{
			for (int i = _entries.Count - 1; i >= 0; i--)
			{
				_entries.RemoveAt(i);
				_foldout.RemoveAt(i);
			}

			RefreshLabel();
			MarkDirtyRepaint();
		}

		public ICollectionEntryWidget GetEntry(int index)
		{
			return _entries.IsValidIndex(index) ? _entries[index] : null;
		}

		public void Remove(int index)
		{
			_entries[index].ValueChanged -= OnItemReplaced;
			_entries[index].DeleteRequested -= OnDeleteRequested;
			CleanupEntry();
			_entries.RemoveAt(index);
			_foldout.RemoveAt(index);
			for (int i = index; i < _entries.Count; i++)
			{
				_entries[i].Index -= 1;
			}

			RefreshLabel();
			MarkDirtyRepaint();
		}

		public void Replace(int index, string newValue)
		{
			_entries[index].Value = newValue;
			MarkDirtyRepaint();
		}

		protected virtual void CleanupEntry() { }
		protected abstract ICollectionEntryWidget MakeEntry(int index, string value, VisualElement root);

		protected void NotifyValueProposed(string valueProposed)
		{
			ValueProposed?.Invoke(valueProposed);
		}

		private void OnAddRequested()
		{
			CollectionEvent colEvent = CollectionEvent.Add(_entries.Count, string.Empty);
			ValueChanged?.Invoke(colEvent.ToString());
		}

		private void OnClearRequested()
		{
			ValueChanged?.Invoke(CollectionEvent.Clear.ToString());
		}

		private void OnDeleteRequested(int index)
		{
			CollectionEvent colEvent = CollectionEvent.Remove(index);
			ValueChanged?.Invoke(colEvent.ToString());
		}

		private void OnItemReplaced(string newValue)
		{
			ValueChanged?.Invoke(newValue);
		}

		private void RefreshLabel()
		{
			_foldout.text = $"Collection ({_entries.Count})";
		}
	}
}
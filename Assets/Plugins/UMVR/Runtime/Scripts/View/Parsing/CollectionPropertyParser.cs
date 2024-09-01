using System;
using System.Collections.Generic;
using pindwin.umvr.Model;
using pindwin.umvr.View.Binding;

namespace pindwin.umvr.View.Parsing
{
	public sealed class CollectionPropertyParser<TItem> : PropertyParser
	{
		private readonly PropertyParser _itemParser;

		public CollectionPropertyParser(PropertyParserProvider propertyParserProvider)
		{
			_itemParser = propertyParserProvider.GetParser(typeof(TItem));
		}

		public override Type Type => typeof(IList<TItem>);

		protected override bool TryParse<TProperty>(string value, TProperty property)
		{
			if (_itemParser == null)
			{
				return false;
			}
			
			if (property is CollectionProperty<TItem> colProperty)
			{
				CollectionEvent ev = CollectionEvent.FromString(value);
				switch (ev.Type)
				{
					case CollectionEventType.Add:
						colProperty.Collection.Add(default);
						break;
					case CollectionEventType.Remove:
						colProperty.Collection.RemoveAt(ev.Index);
						break;
					case CollectionEventType.Replace:
						_itemParser.Parse(ev.Value, new CollectionValueContainerAdapter<TItem>(colProperty, ev.Index));
						break;
					case CollectionEventType.Clear:
						colProperty.Collection.Clear();
						break;
				}
				return true;
			}
			
			return false;
		}

		public override bool IsValid(string payload)
		{
			if (_itemParser == null)
			{
				return false;
			}

			try
			{
				CollectionEvent ev = CollectionEvent.FromString(payload);
				return _itemParser.IsValid(ev.Value);
			}
			catch
			{
				return false;
			}
		}
	}
    
	internal class CollectionValueContainerAdapter<TItem> : IValueContainer<TItem>
	{
		private readonly CollectionProperty<TItem> _collection;
		private readonly int _index;

		public CollectionValueContainerAdapter(CollectionProperty<TItem> collection, int index)
		{
			_collection = collection;
			_index = index;
		}

		public TItem Value
		{
			get => _collection.Collection[_index];
			set => _collection.Collection[_index] = value;
		}
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using pindwin.development;
using UniRx;

namespace pindwin.umvr.Model
{
	public class CollectionProperty<TItem> : CollectionProperty
	{
		public ReactiveCollection<TItem> Collection { get; }
		protected CompositeDisposable CompositeDisposable { get; }

		public CollectionProperty(string label) : this(label, null)
		{ }

		public CollectionProperty(string label, IEnumerable<TItem> items) : this(label, new ReactiveCollection<TItem>(), items)
		{ }

		public CollectionProperty(string label, ReactiveCollection<TItem> collection, IEnumerable<TItem> items) : base(label, typeof(TItem))
		{
			Collection = collection.AssertNotNull();
			Collection.Clear();

			CompositeDisposable = new CompositeDisposable
			{
				Collection.ObserveAdd().Subscribe(_ => NotifyChanged()),
				Collection.ObserveRemove().Subscribe(_ => NotifyChanged()),
				Collection.ObserveReset().Subscribe(_ => NotifyChanged()),
				Collection.ObserveMove().Subscribe(_ => NotifyChanged()),
				Collection.ObserveReplace().Subscribe(_ => NotifyChanged())
			};

			if (items == null)
			{
				return;
			}
			
			foreach (TItem item in items)
			{
				Collection.Add(item);
			}
		}

		public override void Dispose()
		{
			CompositeDisposable.Dispose();
			Collection?.Dispose();
		}

		public override string ToString()
		{
			return $"{typeof(TItem)} collection: {(Collection != null ? string.Join(",", Collection) : "null")}";
		}

		public override IEnumerator GetEnumerator()
		{
			return Collection.GetEnumerator();
		}
	}

	public abstract class CollectionProperty : Property, IEnumerable
	{
		protected CollectionProperty(string label, Type type)
			: base(label, type) { }

		public abstract IEnumerator GetEnumerator();
	}
}
using System.Collections.Generic;
using pindwin.development;
using UniRx;

namespace pindwin.umvr.Model
{
	public class CollectionProperty<TItem> : Property
	{
		public ReactiveCollection<TItem> Collection { get; }
		protected CompositeDisposable CompositeDisposable { get; }

		public CollectionProperty() : this(null)
		{ }

		public CollectionProperty(IEnumerable<TItem> items) : this(new ReactiveCollection<TItem>(), items)
		{ }

		public CollectionProperty(ReactiveCollection<TItem> collection, IEnumerable<TItem> items)
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
	}
}
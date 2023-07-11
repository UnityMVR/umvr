using System.Collections.Generic;
using UniRx;

namespace pindwin.umvr.Model
{
	public sealed class ModelCollectionProperty<TModel> : CollectionProperty<TModel>
		where TModel : class, IModel
	{
		public ModelCollectionProperty(string label)
			: this(label, new ReactiveCollection<TModel>(), null) { }

		public ModelCollectionProperty(string label, IEnumerable<TModel> items)
			: this(label, new ReactiveCollection<TModel>(), items) { }

		public ModelCollectionProperty(string label, ReactiveCollection<TModel> collection, IEnumerable<TModel> items)
			: base(label, collection, items)
		{
			foreach (TModel model in Collection)
			{
				HookDisposeHandler(model);
			}

			CompositeDisposable.Add(new CompositeDisposable
			{
				Collection.ObserveAdd().Subscribe(OnAdd),
				Collection.ObserveRemove().Subscribe(OnRemove),
				Collection.ObserveReset().Subscribe(OnReset),
				Collection.ObserveReplace().Subscribe(OnReplace)
			});
		}

		public override void Dispose()
		{
			foreach (TModel item in Collection)
			{
				UnhookDisposeHandler(item);
			}

			base.Dispose();
		}

		private void OnAdd(CollectionAddEvent<TModel> added)
		{
			HookDisposeHandler(added.Value);
		}

		private void OnRemove(CollectionRemoveEvent<TModel> removed)
		{
			UnhookDisposeHandler(removed.Value);
		}

		private void OnReplace(CollectionReplaceEvent<TModel> replace)
		{
			UnhookDisposeHandler(replace.OldValue);
			HookDisposeHandler(replace.NewValue);
		}

		private void OnReset(Unit obj)
		{
			foreach (TModel item in Collection)
			{
				UnhookDisposeHandler(item);
			}
		}

		private void HookDisposeHandler(TModel item)
		{
			if (item == null)
			{
				return;
			}
			
			item.Disposing += CleanupRoutine;
		}

		private void UnhookDisposeHandler(TModel item)
		{
			if (item == null)
			{
				return;
			}
			
			item.Disposing -= CleanupRoutine;
		}

		private void CleanupRoutine(IModel disposed)
		{
			if (disposed is IModelCleanup model)
			{
				model.AddCleanupHandler(m => Collection.Remove(m as TModel), CleanupPriority.High);
			}
		}
	}
}
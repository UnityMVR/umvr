using System;
using System.Collections.Generic;
using System.Reflection;
using pindwin.development;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using UniRx;
using UnityEngine;

namespace pindwin.umvr.Repository
{
	public class SecondaryIndex<TIndex, TModel> : ISecondaryIndex<TModel>
		where TModel : IModel, IEnumerable<string>
	{
		private readonly IRepository<TModel> _repository;
		private readonly Dictionary<TIndex, TModel> _index;
		private readonly PropertyInfo _indexedProperty;

		private bool? _isProperty;

		private readonly DictionaryDisposable<Id, IDisposable> _subscriptions;

		public string PropertyName { get; }

		public SecondaryIndex(string name, IRepository<TModel> repository)
		{
			PropertyName = name;
			_repository = repository.AssertNotNull();
			_index = new Dictionary<TIndex, TModel>();
			_subscriptions = new DictionaryDisposable<Id, IDisposable>();
			_indexedProperty = typeof(TModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
			if (_indexedProperty == null)
			{
				throw new UMVRException(
					$"Attempted adding a secondary index for non-existing property: {typeof(TModel)}.{name}"
				);
			}

			_repository.Added += Add;
			_repository.Removed += Remove;
		}

		public TModel Get(object id)
		{
			return _index.TryGetValue((TIndex) id, out TModel value) ? value : default;
		}

		public void Add(TModel model)
		{
			ObserveIndex(model);
			_index.Add(GetIndex(model), model);
		}

		public void Remove(TModel model)
		{
			if (_subscriptions.TryGetValue(model.Id, out IDisposable subscription))
			{
				subscription.Dispose();
			}
			_index.Remove(GetIndex(model));
		}

		private TIndex GetIndex(TModel instance)
		{
			return (TIndex) _indexedProperty.GetValue(instance);
		}

		private void RefreshIndex(IndexChange change)
		{
			TModel item = _index[change.Previous];
			_index.Remove(change.Previous);
			_index.Add(change.Latest, item);
		}

		private void ObserveIndex(TModel model)
		{
			if (_isProperty.HasValue == false)
			{
				_isProperty = false;
				foreach (string label in model)
				{
					if (label.Equals(PropertyName, StringComparison.OrdinalIgnoreCase))
					{
						_isProperty = true;
						break;
					}
				}
				
				if (_isProperty == false)
				{
					Debug.LogWarning($"Secondary index for property: {typeof(TModel)}.{PropertyName} will never be updated!");
				}
			}

			if (_isProperty == false)
			{
				return;
			}

			IObservable<TIndex> previous = model.GetProperty<TIndex>(PropertyName);
			IObservable<TIndex> latest = previous.Skip(1);
			IDisposable refresh = latest
				.Zip(previous, (l, p) => new IndexChange(l, p))
				.Subscribe(RefreshIndex);
			_subscriptions.Add(model.Id, refresh);
		}

		public void Dispose()
		{
			_subscriptions.Dispose();
			_repository.Added -= Add;
			_repository.Removed -= Remove;
		}

		private readonly struct IndexChange
		{
			public readonly TIndex Latest;
			public readonly TIndex Previous;

			public IndexChange(TIndex latest, TIndex previous)
			{
				Latest = latest;
				Previous = previous;
			}
		}
	}
}
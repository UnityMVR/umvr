using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using pindwin.development;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using pindwin.umvr.Reactor;

namespace pindwin.umvr.Repository
{
	[UsedImplicitly]
	public class Repository<TEntity, TConcrete, TReactor> : IRepository<TEntity>
		where TEntity : IModel
		where TConcrete : Model<TConcrete>, TEntity
		where TReactor : Reactor<TConcrete>
	{
		private bool _disposing;
		private readonly List<TConcrete> _list;
		private readonly Queue<SuppressedNotification> _notificationsQueue;
		private readonly ReactorFactory<TConcrete, TReactor> _reactorFactory;
		private bool _suppressNotifications;
		
		private readonly Dictionary<Id, TEntity> _mainIndex;
		private readonly Dictionary<string, ISecondaryIndex<TEntity>> _secondaryIndexes;
		
		public event Action<TEntity> Added;
		public event Action<TEntity> Removed;
		
		public IReadOnlyList<TEntity> Entities => _list;
		protected internal IReadOnlyList<TConcrete> ConcreteEntities => _list;

		public event Action CountChanged;
		public Type StoredType => typeof(TConcrete);

		public bool SuppressNotifications
		{
			get => _suppressNotifications;
			set
			{
				_suppressNotifications = value;
				if (_suppressNotifications)
				{
					return;
				}

				while (_notificationsQueue.Count > 0)
				{
					SuppressedNotification next = _notificationsQueue.Dequeue();
					if (next.IsAdd)
					{
						TryNotifyAdded(next.Model);
					}
					else
					{
						TryNotifyRemoved(next.Model);
					}
				}
			}
		}

		protected Repository(ReactorFactory<TConcrete, TReactor> presenterFactory)
		{
			_list = new List<TConcrete>();
			_notificationsQueue = new Queue<SuppressedNotification>();
			_reactorFactory = presenterFactory.AssertNotNull();
			_mainIndex = new Dictionary<Id, TEntity>();
			_secondaryIndexes = new Dictionary<string, ISecondaryIndex<TEntity>>();
		}

		public void Add(IModel model)
		{
			Add(model as TConcrete);
		}

		public void Remove(IModel model)
		{
			Remove(model as TConcrete);
		}

		public void Add(TEntity model)
		{
			if (model is TConcrete concrete)
			{
				if (model.IsValid() == false)
				{
					throw new UMVRInvalidOperationException(model.GetType());
				}

				_list.Add(concrete);
				TryNotifyAdded(concrete);
				return;
			}
			
			throw new UMVRDataTypeMismatchException(typeof(TEntity), typeof(TConcrete), model.GetType());
		}

		public void Remove(TEntity model)
		{
			if (model is TConcrete concrete)
			{
				if (model.IsValid() == false)
				{
					throw new UMVRInvalidOperationException(model.GetType());
				}

				if (_list.Remove(concrete) == false)
				{
					return;
				}

				TryNotifyRemoved(concrete);
				return;
			}
			
			throw new UMVRDataTypeMismatchException(typeof(TEntity), typeof(TConcrete), model.GetType());
		}

		public TEntity Get(Id id)
		{
			return _mainIndex.TryGetValue(id, out TEntity value) ? value : default;
		}

		public TEntity GetBy(string indexName, object index)
		{
			if (_secondaryIndexes.TryGetValue(indexName, out var secIndex))
			{
				return secIndex.Get(index);
			}

			return default;
		}

		public IEnumerator<IModel> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected void AddIndex(string key, ISecondaryIndex<TEntity> index)
		{
			_secondaryIndexes.Add(key, index);
			foreach (TEntity entity in Entities)
			{
				index.Add(entity);
			}
		}

		protected void RemoveIndex(string key)
		{
			if(_secondaryIndexes.TryGetValue(key, out ISecondaryIndex<TEntity> index))
			{
				_secondaryIndexes.Remove(key);
				index.Dispose();
			}
		}

		private void OnEntityDisposing(IModel entity)
		{
			if (_disposing)
			{
				return;
			}

			if (entity is TEntity model)
			{
				Remove(model);
			}
		}

		private void TryNotifyAdded(TConcrete concrete)
		{
			if (SuppressNotifications)
			{
				_notificationsQueue.Enqueue(new SuppressedNotification(true, concrete));
				return;
			}
			
			_mainIndex.Add(concrete.Id, concrete);
			concrete.AddCleanupHandler(OnEntityDisposing, CleanupPriority.Medium);
			_reactorFactory.Create(concrete);
			Added?.Invoke(concrete);
			CountChanged?.Invoke();
		}
		
		private void TryNotifyRemoved(TConcrete concrete)
		{
			if (SuppressNotifications)
			{
				_notificationsQueue.Enqueue(new SuppressedNotification(false, concrete));
				return;
			}
			
			_mainIndex.Remove(concrete.Id);
			Removed?.Invoke(concrete);
			CountChanged?.Invoke();
		}

		public void Dispose()
		{
			_disposing = true;
			foreach (TEntity entity in Entities)
			{
				entity.Dispose();
			}
		}

		private class SuppressedNotification
		{
			public readonly bool IsAdd;
			public readonly TConcrete Model;

			public SuppressedNotification(bool isAdd, TConcrete model)
			{
				IsAdd = isAdd;
				Model = model;
			}
		}
	}
}
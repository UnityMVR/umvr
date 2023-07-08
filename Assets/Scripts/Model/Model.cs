using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using pindwin.umvr.Exceptions;
using UniRx;

namespace pindwin.umvr.Model
{
	public abstract class Model<TModel> : IModelCleanup, IModel, IEnumerable<string>
		where TModel : class, IModel
	{
		public event Action<IModel> Disposing;
		public Id Id { get; private set; }
		
		private static FieldInfo[] _fields;
		private static List<string> _labels;

		private Dictionary<string, Property> _collections;
		private Dictionary<string, Property> _observables;
		private readonly List<ModelCleanupStep> _cleanup;
		private Action _cleanupRoutines;
		private bool _disposing;

		protected Model(Id id)
		{
			Id = id == Id.DEFAULT ? Id.Next() : id;
			_cleanup = new List<ModelCleanupStep>();
		}

		public ReactiveCollection<TItemType> GetCollection<TItemType>(string label)
		{
			if (_collections.TryGetValue(label.ToLowerInvariant(), out Property value))
			{
				return ((CollectionProperty<TItemType>)value).Collection;
			}

			throw new UMVRDataBindingException(typeof(IList<TItemType>), label, GetType());
		}

		public IObservable<TProperty> GetProperty<TProperty>(string label)
		{
			if (_observables.TryGetValue(label.ToLowerInvariant(), out Property value))
			{
				return (IObservable<TProperty>)value;
			}

			throw new UMVRDataBindingException(typeof(TProperty), label, GetType());
		}

		public Property GetProperty(string label)
		{
			label = label.ToLower();
			
			if (_observables.TryGetValue(label, out Property single))
			{
				return single;
			}

			if (_collections.TryGetValue(label, out Property collection))
			{
				return collection;
			}

			throw new UMVRException($"Property {label} does not exist!");
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _labels.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			return $"{GetType().Name}#{Id}";
		}

		public void Dispose()
		{
			if (_disposing)
			{
				return;
			}

			_disposing = true;
			Disposing?.Invoke(this);
			InvokeCleanupHandlers(this);

			foreach (string label in _labels)
			{
				try
				{
					Property p = GetProperty(label);
					p?.Dispose();
				}
				catch (UMVRException)
				{ }
			}

			Id = Id.UNKNOWN;
			_observables.Clear();
			_collections.Clear();
		}

		internal void AddCleanupHandler(Action<IModel> handler, int priority)
		{
			(this as IModelCleanup).AddCleanupHandler(handler, priority);
		}
		
		void IModelCleanup.AddCleanupHandler(Action<IModel> handler, int priority)
		{
			_cleanup.Add(new ModelCleanupStep(handler, priority));
		}

		private void InvokeCleanupHandlers(IModel model)
		{
			_cleanup.Sort();
			for (int i = _cleanup.Count - 1; i >= 0; i--)
			{
				_cleanup[i].Action?.Invoke(model);
			}
		}

		protected void CascadeDispose(IModel disposing)
		{
			Dispose();
		}

		protected void RegisterDataStreams(TModel model)
		{
			if (_fields == null)
			{
				Type mType = typeof(TModel);
				_fields = mType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
			}

			_observables = MapPropertiesOfGenericType(model, typeof(SingleProperty<>));
			_collections = MapPropertiesOfGenericType(model, typeof(CollectionProperty<>));

			_labels ??= _observables.Keys.Concat(_collections.Keys).ToList();
		}

		private static Dictionary<string, Property> MapPropertiesOfGenericType(TModel model, Type desiredType)
		{
			Dictionary<string, Property> result = new Dictionary<string, Property>();
			foreach (FieldInfo fieldInfo in _fields)
			{
				Type t = fieldInfo.FieldType;
				if (IsSubclassOfRawGeneric(desiredType, t) == false)
				{
					continue;
				}

				result[fieldInfo.Name.ToLower().TrimStart('_')] =
					fieldInfo.GetValue(model) as Property ?? 
					throw new UMVRDataBindingException(t, fieldInfo.Name, typeof(TModel));
			}

			return result;
		}

		private static bool IsSubclassOfRawGeneric(Type genericType, Type comparedType)
		{
			while (comparedType != null && comparedType != typeof(object))
			{
				Type currentType = comparedType.IsGenericType ? comparedType.GetGenericTypeDefinition() : comparedType;
				if (genericType == currentType)
				{
					return true;
				}

				comparedType = comparedType.BaseType;
			}

			return false;
		}
	}
}
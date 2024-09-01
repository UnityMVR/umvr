using System;
using System.Collections.Generic;
using pindwin.umvr.Exceptions;

namespace pindwin.umvr.Model
{
	public sealed class GenericModelFactory
	{
		private readonly Dictionary<Type, IModelFactory> _factories;

		public GenericModelFactory(List<IModelFactory> factories)
		{
			_factories = new Dictionary<Type, IModelFactory>();
			foreach (IModelFactory factory in factories)
			{
				_factories[factory.ModelType] = factory;
			}
		}

		public IModel CreateModel<TModel>()
			where TModel : class, IModel
		{
			return CreateModel(typeof(TModel));
		}

		public IModel CreateModel(Type modelType)
		{
			return _factories.TryGetValue(modelType, out IModelFactory factory)
				? factory.CreateDefault()
				: throw new UMVRCantLocateResourceException(modelType, GetType());
		}
	}
}
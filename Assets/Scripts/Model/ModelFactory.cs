using System;
using JetBrains.Annotations;
using pindwin.development;
using pindwin.umvr.Exceptions;
using pindwin.umvr.Repository;
using pindwin.umvr.Serialization;
using Zenject;

namespace pindwin.umvr.Model
{
	[UsedImplicitly]
	public class ModelFactory<TModel, TConcreteModel>
		: PlaceholderFactory<Id, TConcreteModel>, IModelFactory<TModel>
		where TModel : IModel
		where TConcreteModel : Model<TConcreteModel>, TModel
	{
		protected IRepository<TModel> Repository { get; }

		private readonly ISerializer<TConcreteModel> _serializer;

		protected ModelFactory(IRepository<TModel> repository)
			: this(repository, null) { }

		protected ModelFactory(IRepository<TModel> repository, ISerializer<TConcreteModel> serializer)
		{
			Repository = repository.AssertNotNull();
			_serializer = serializer ?? new NullSerializer<TConcreteModel>();
		}

		public Type ModelType => typeof(TConcreteModel);

		public new TModel Create(Id id)
		{
			TConcreteModel product = Instantiate(id);
			Repository.Add(product);
			return product;
		}

		public virtual TModel Create()
		{
			return Create(Id.Next());
		}

		public TModel Create<TDeserializationPayload>(TDeserializationPayload serializationPayload)
		{
			if (_serializer is ISerializer<TDeserializationPayload, TConcreteModel> serializer)
			{
				TConcreteModel model = Instantiate(serializer.DeserializeId(serializationPayload));
				serializer.DeserializeBody(model, serializationPayload);
				Repository.Add(model);
				return model;
			}

			throw new UMVRException(
				string.Format(
					UMVRException.SerializationSetupErrorFormat,
					typeof(TConcreteModel),
					typeof(ISerializer<TDeserializationPayload, TConcreteModel>),
					_serializer.GetType()));
		}

		protected virtual TConcreteModel Instantiate(Id id)
		{
			return base.Create(id);
		}
	}

	[UsedImplicitly]
	public class ModelFactory<TModel, TConcreteModel, TParam0>
		: PlaceholderFactory<Id, TParam0, TConcreteModel>, IModelFactory<TModel, TParam0>
		where TModel : IModel
		where TConcreteModel : Model<TConcreteModel>, TModel
	{
		protected IRepository<TModel> Repository { get; }

		private readonly ISerializer<TConcreteModel> _serializer;

		protected ModelFactory(IRepository<TModel> repository)
			: this(repository, null) { }

		protected ModelFactory(IRepository<TModel> repository, ISerializer<TConcreteModel> serializer)
		{
			Repository = repository.AssertNotNull();
			_serializer = serializer ?? new NullSerializer<TConcreteModel>();
		}

		public Type ModelType => typeof(TConcreteModel);

		public new TModel Create(Id id, TParam0 param0)
		{
			TConcreteModel product = Instantiate(id, param0);
			Repository.Add(product);
			return product;
		}

		public virtual TModel Create(TParam0 param0)
		{
			return Create(Id.Next(), param0);
		}

		public TModel Create<TDeserializationPayload>(TDeserializationPayload serializationPayload)
		{
			if (_serializer is ISerializer<TDeserializationPayload, TConcreteModel> serializer)
			{
				TConcreteModel model = Instantiate(serializer.DeserializeId(serializationPayload), default);
				serializer.DeserializeBody(model, serializationPayload);
				Repository.Add(model);
				return model;
			}

			throw new UMVRException(
				string.Format(
					UMVRException.SerializationSetupErrorFormat,
					typeof(TConcreteModel),
					typeof(ISerializer<TDeserializationPayload, TConcreteModel>),
					_serializer.GetType()));
		}

		protected virtual TConcreteModel Instantiate(Id id, TParam0 param0)
		{
			return base.Create(id, param0);
		}
	}

	[UsedImplicitly]
	public class ModelFactory<TModel, TConcreteModel, TParam0, TParam1>
		: PlaceholderFactory<Id, TParam0, TParam1, TConcreteModel>, IModelFactory<TModel, TParam0, TParam1>
		where TModel : IModel
		where TConcreteModel : Model<TConcreteModel>, TModel
	{
		protected IRepository<TModel> Repository { get; }

		private readonly ISerializer<TConcreteModel> _serializer;

		protected ModelFactory(IRepository<TModel> repository)
			: this(repository, null) { }

		protected ModelFactory(IRepository<TModel> repository, ISerializer<TConcreteModel> serializer)
		{
			Repository = repository.AssertNotNull();
			_serializer = serializer ?? new NullSerializer<TConcreteModel>();
		}

		public Type ModelType => typeof(TConcreteModel);

		public new TModel Create(Id id, TParam0 param0, TParam1 param1)
		{
			TConcreteModel product = Instantiate(id, param0, param1);
			Repository.Add(product);
			return product;
		}

		public virtual TModel Create(TParam0 param0, TParam1 param1)
		{
			return Create(Id.Next(), param0, param1);
		}

		public TModel Create<TDeserializationPayload>(TDeserializationPayload serializationPayload)
		{
			if (_serializer is ISerializer<TDeserializationPayload, TConcreteModel> serializer)
			{
				TConcreteModel model = Instantiate(serializer.DeserializeId(serializationPayload), default, default);
				serializer.DeserializeBody(model, serializationPayload);
				return model;
			}

			throw new UMVRException(
				string.Format(
					UMVRException.SerializationSetupErrorFormat,
					typeof(TConcreteModel),
					typeof(ISerializer<TDeserializationPayload, TConcreteModel>),
					_serializer.GetType()));
		}

		protected virtual TConcreteModel Instantiate(Id id, TParam0 param0, TParam1 param1)
		{
			return base.Create(id, param0, param1);
		}
	}

	[UsedImplicitly]
	public class ModelFactory<TModel, TConcreteModel, TParam0, TParam1, TParam2>
		: PlaceholderFactory<Id, TParam0, TParam1, TParam2, TConcreteModel>,
			IModelFactory<TModel, TParam0, TParam1, TParam2>
		where TModel : IModel
		where TConcreteModel : Model<TConcreteModel>, TModel
	{
		protected IRepository<TModel> Repository { get; }

		private readonly ISerializer<TConcreteModel> _serializer;

		protected ModelFactory(IRepository<TModel> repository)
			: this(repository, null) { }

		protected ModelFactory(IRepository<TModel> repository, ISerializer<TConcreteModel> serializer)
		{
			Repository = repository.AssertNotNull();
			_serializer = serializer ?? new NullSerializer<TConcreteModel>();
		}

		public Type ModelType => typeof(TConcreteModel);

		public new TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2)
		{
			TConcreteModel product = Instantiate(id, param0, param1, param2);
			Repository.Add(product);
			return product;
		}

		public virtual TModel Create(TParam0 param0, TParam1 param1, TParam2 param2)
		{
			return Create(Id.Next(), param0, param1, param2);
		}

		public TModel Create<TDeserializationPayload>(TDeserializationPayload serializationPayload)
		{
			if (_serializer is ISerializer<TDeserializationPayload, TConcreteModel> module)
			{
				TConcreteModel model =
					Instantiate(module.DeserializeId(serializationPayload), default, default, default);
				module.DeserializeBody(model, serializationPayload);
				Repository.Add(model);
				return model;
			}

			throw new UMVRException(
				string.Format(
					UMVRException.SerializationSetupErrorFormat,
					typeof(TConcreteModel),
					typeof(ISerializer<TDeserializationPayload, TConcreteModel>),
					_serializer.GetType()));
		}

		protected virtual TConcreteModel Instantiate(Id id, TParam0 param0, TParam1 param1, TParam2 param2)
		{
			return base.Create(id, param0, param1, param2);
		}
	}

	[UsedImplicitly]
	public class ModelFactory<TModel, TConcreteModel, TParam0, TParam1, TParam2, TParam3>
		: PlaceholderFactory<Id, TParam0, TParam1, TParam2, TParam3, TConcreteModel>,
			IModelFactory<TModel, TParam0, TParam1, TParam2, TParam3>
		where TModel : IModel
		where TConcreteModel : Model<TConcreteModel>, TModel
	{
		protected IRepository<TModel> Repository { get; }

		private readonly ISerializer<TConcreteModel> _serializer;

		protected ModelFactory(IRepository<TModel> repository)
			: this(repository, null) { }

		protected ModelFactory(IRepository<TModel> repository, ISerializer<TConcreteModel> serializer)
		{
			Repository = repository.AssertNotNull();
			_serializer = serializer ?? new NullSerializer<TConcreteModel>();
		}

		public Type ModelType => typeof(TConcreteModel);

		public new TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3)
		{
			TConcreteModel product = Instantiate(id, param0, param1, param2, param3);
			Repository.Add(product);
			return product;
		}

		public virtual TModel Create(TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3)
		{
			return Create(Id.Next(), param0, param1, param2, param3);
		}

		public TModel Create<TDeserializationPayload>(TDeserializationPayload serializationPayload)
		{
			if (_serializer is ISerializer<TDeserializationPayload, TConcreteModel> serializer)
			{
				TConcreteModel model = Instantiate(serializer.DeserializeId(serializationPayload), default, default,
												   default, default);
				serializer.DeserializeBody(model, serializationPayload);
				Repository.Add(model);
				return model;
			}

			throw new UMVRException(
				string.Format(
					UMVRException.SerializationSetupErrorFormat,
					typeof(TConcreteModel),
					typeof(ISerializer<TDeserializationPayload, TConcreteModel>),
					_serializer.GetType()));
		}

		protected virtual TConcreteModel Instantiate(
			Id id,
			TParam0 param0,
			TParam1 param1,
			TParam2 param2,
			TParam3 param3)
		{
			return base.Create(id, param0, param1, param2, param3);
		}
	}

	[UsedImplicitly]
	public class ModelFactory<TModel, TConcreteModel, TParam0, TParam1, TParam2, TParam3, TParam4>
		: PlaceholderFactory<Id, TParam0, TParam1, TParam2, TParam3, TParam4, TConcreteModel>,
			IModelFactory<TModel, TParam0, TParam1, TParam2, TParam3, TParam4>
		where TModel : IModel
		where TConcreteModel : Model<TConcreteModel>, TModel
	{
		protected IRepository<TModel> Repository { get; }

		private readonly ISerializer<TConcreteModel> _serializer;

		protected ModelFactory(IRepository<TModel> repository)
			: this(repository, null) { }

		protected ModelFactory(IRepository<TModel> repository, ISerializer<TConcreteModel> serializer)
		{
			Repository = repository.AssertNotNull();
			_serializer = serializer ?? new NullSerializer<TConcreteModel>();
		}

		public Type ModelType => typeof(TConcreteModel);

		public new TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
		{
			TConcreteModel product = Instantiate(id, param0, param1, param2, param3, param4);
			Repository.Add(product);
			return product;
		}

		public virtual TModel Create(TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
		{
			return Create(Id.Next(), param0, param1, param2, param3, param4);
		}

		public TModel Create<TDeserializationPayload>(TDeserializationPayload serializationPayload)
		{
			if (_serializer is ISerializer<TDeserializationPayload, TConcreteModel> serializer)
			{
				TConcreteModel model = Instantiate(serializer.DeserializeId(serializationPayload), default, default,
												   default, default, default);
				serializer.DeserializeBody(model, serializationPayload);
				Repository.Add(model);
				return model;
			}

			throw new UMVRException(
				string.Format(
					UMVRException.SerializationSetupErrorFormat,
					typeof(TConcreteModel),
					typeof(ISerializer<TDeserializationPayload, TConcreteModel>),
					_serializer.GetType()));
		}

		protected virtual TConcreteModel Instantiate(
			Id id,
			TParam0 param0,
			TParam1 param1,
			TParam2 param2,
			TParam3 param3,
			TParam4 param4)
		{
			return base.Create(id, param0, param1, param2, param3, param4);
		}
	}
}
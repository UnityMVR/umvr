using System;

namespace pindwin.umvr.Model
{
	public interface IModelFactory
	{
		IModel CreateDefault();
		Type ModelType { get; }
	}

	public interface IModelFactory<out TModel> : IModelFactory
		where TModel : IModel
	{
		TModel Create(Id id);
		TModel Create();
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}

	public interface IModelFactory<out TModel, in TParam0> : IModelFactory
		where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0);
		TModel Create(TParam0 param0);
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}

	public interface IModelFactory<out TModel, in TParam0, in TParam1> : IModelFactory
		where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1);
		TModel Create(TParam0 param0, TParam1 param1);
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}

	public interface IModelFactory<out TModel, in TParam0, in TParam1, in TParam2> : IModelFactory
		where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2);
		TModel Create(TParam0 param0, TParam1 param1, TParam2 param2);
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}

	public interface IModelFactory<out TModel, in TParam0, in TParam1, in TParam2, in TParam3> : IModelFactory
		where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3);
		TModel Create(TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3);
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}

	public interface
		IModelFactory<out TModel, in TParam0, in TParam1, in TParam2, in TParam3, in TParam4> : IModelFactory
		where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
		TModel Create(TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
}
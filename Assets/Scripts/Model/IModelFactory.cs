using System;
using pindwin.umvr.Serialization;

namespace pindwin.umvr.Model
{
	public interface IModelFactory<out TModel> where TModel : IModel
	{
		TModel Create(Id id);
		TModel Create();
		Type ModelType { get; }
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
	
	public interface IModelFactory<out TModel, in TParam0> where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0);
		TModel Create(TParam0 param0);
		Type ModelType { get; }
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
	
	public interface IModelFactory<out TModel, in TParam0, in TParam1> where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1);
		TModel Create(TParam0 param0, TParam1 param1);
		Type ModelType { get; }
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
	
	public interface IModelFactory<out TModel, in TParam0, in TParam1, in TParam2> where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2);
		TModel Create(TParam0 param0, TParam1 param1, TParam2 param2);
		Type ModelType { get; }
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
	
	public interface IModelFactory<out TModel, in TParam0, in TParam1, in TParam2, in TParam3> where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3);
		TModel Create(TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3);
		Type ModelType { get; }
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
	
	public interface IModelFactory<out TModel, in TParam0, in TParam1, in TParam2, in TParam3, in TParam4> where TModel : IModel
	{
		TModel Create(Id id, TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
		TModel Create(TParam0 param0, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
		Type ModelType { get; }
		TModel Create<TDeserializationPayload>(TDeserializationPayload deserializationPayload);
	}
}
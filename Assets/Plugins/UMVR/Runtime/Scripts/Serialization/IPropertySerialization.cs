using System;

namespace pindwin.umvr.Model
{
	public interface IPropertySerialization<in TSerializationArg, in TDeserializationArg, in TConcreteModel>
		where TConcreteModel : class, IModel
	{
		string Name { get; }
		Action<TSerializationArg, TConcreteModel> Get { get; }
		Action<TDeserializationArg, TConcreteModel> Set { get; }
	}
}
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace pindwin.umvr.Serialization
{
	public interface ISerializer<TConcreteModel>
		where TConcreteModel : class, IModel
	{ }

	public interface ISerializer<in TDeserializationPayload, TConcreteModel> : ISerializer<TConcreteModel>
		where TConcreteModel : class, IModel
	{
		Id DeserializeId(TDeserializationPayload serializationPayload);
		void DeserializeBody(TConcreteModel model, TDeserializationPayload serializationPayload);
	}

	public interface ISerializer<TSerializationPayload, TDeserializationPayload, TConcreteModel> : ISerializer<TDeserializationPayload, TConcreteModel>
		where TConcreteModel : class, IModel
	{
		List<IPropertySerialization<TSerializationPayload, TDeserializationPayload, TConcreteModel>> PropertySerializers { get; }
		void Serialize(TConcreteModel model, TSerializationPayload serializationPayload);
	}
}
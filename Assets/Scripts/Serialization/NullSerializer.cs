using System;
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace pindwin.umvr.Serialization
{
	public class
		NullSerializer<TConcreteModel> : ISerializer<NullSerializationParam, NullDeserializationParam, TConcreteModel>
		where TConcreteModel : class, IModel
	{
		public List<IPropertySerialization<NullSerializationParam, NullDeserializationParam, TConcreteModel>>
			PropertySerializers { get; }

		public Type TypeId => typeof(TConcreteModel);

		public void DeserializeBody(TConcreteModel model, NullDeserializationParam serializationPayload) { }

		public Id DeserializeId(NullDeserializationParam serializationPayload)
		{
			return default;
		}

		public void Serialize(TConcreteModel model, NullSerializationParam serializationPayload) { }
	}

	public class NullSerializationParam { }

	public class NullDeserializationParam { }
}
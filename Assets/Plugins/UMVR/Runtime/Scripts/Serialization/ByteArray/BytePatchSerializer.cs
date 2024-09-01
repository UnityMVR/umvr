using System;
using System.Collections.Generic;
using pindwin.umvr.Model;

namespace pindwin.umvr.Serialization.BytePatch
{
	public class BytePatchSerializer<TConcreteModel> : ISerializer<BytePatchSerializationPayload, BytePatchDeserializationPayload, TConcreteModel>
		where TConcreteModel : class, IModel
	{
		public Type TypeId => typeof(TConcreteModel);

		public List<IPropertySerialization<BytePatchSerializationPayload, BytePatchDeserializationPayload, TConcreteModel>> PropertySerializers { get; }

		public void Serialize(TConcreteModel model, BytePatchSerializationPayload serializationPayload)
		{
			serializationPayload.WriteId(model.Id);
			this.WritePatchByMask(ulong.MaxValue, serializationPayload, model);
		}

		public Id DeserializeId(BytePatchDeserializationPayload serializationPayload)
		{
			return serializationPayload.ReadId();
		}

		public void DeserializeBody(TConcreteModel model, BytePatchDeserializationPayload serializationPayload)
		{
			this.ReadPatchByMask(ulong.MaxValue, serializationPayload, model);
		}

		protected BytePatchSerializer()
		{
			PropertySerializers = new List<IPropertySerialization<BytePatchSerializationPayload, BytePatchDeserializationPayload, TConcreteModel>>();
		}
	}
}
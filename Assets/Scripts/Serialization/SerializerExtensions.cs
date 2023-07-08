using pindwin.umvr.Model;

namespace pindwin.umvr.Serialization
{
	public static class SerializerExtensions
	{
		public static void ReadPatchByMask<TSerializationPayload, TDeserializationPayload, TConcreteModel>(
			this ISerializer<TSerializationPayload, TDeserializationPayload, TConcreteModel> serializer,
			ulong mask,
			TDeserializationPayload payload,
			TConcreteModel target)
			where TConcreteModel : class, IModel
		{
			for (int i = 0, count = serializer.PropertySerializers.Count; i < 64 && i < count; i++)
			{
				if ((mask & (1ul << i)) == 0)
				{
					continue;
				}

				serializer.PropertySerializers[i].Set(payload, target);
			}
		}

		public static void WritePatchByMask<TSerializationPayload, TDeserializationPayload, TConcreteModel>(
			this ISerializer<TSerializationPayload, TDeserializationPayload, TConcreteModel> serializer,
			ulong mask,
			TSerializationPayload payload,
			TConcreteModel target)
			where TConcreteModel : class, IModel
		{
			for (int i = 0, count = serializer.PropertySerializers.Count; i < 64 && i < count; i++)
			{
				if ((mask & (1ul << i)) == 0)
				{
					continue;
				}

				serializer.PropertySerializers[i].Get(payload, target);
			}
		}
	}
}
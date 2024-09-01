using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.Serialization.BytePatch
{
	//todo add auto-bindings of simple properties (will be easier/better with more meta-data held at the Model<TModel> level)
	//todo make it work automatically with patching (set dirty flag OnPropertyChange)
	public class BytePatchPropertySerialization<TConcreteModel>
		: IPropertySerialization<BytePatchSerializationPayload, BytePatchDeserializationPayload, TConcreteModel>
		where TConcreteModel : class, IModel
	{
		public BytePatchPropertySerialization(
			string name,
			Action<BytePatchSerializationPayload, TConcreteModel> get,
			Action<BytePatchDeserializationPayload, TConcreteModel> set)
		{
			Name = name;
			Get = get;
			Set = set;
		}
		
		public string Name { get; }
		public Action<BytePatchSerializationPayload, TConcreteModel> Get { get; }
		public Action<BytePatchDeserializationPayload, TConcreteModel> Set { get; }
	}
}
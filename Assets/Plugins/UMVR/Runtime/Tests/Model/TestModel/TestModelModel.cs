using pindwin.umvr.Model;
using pindwin.umvr.Serialization;
using pindwin.umvr.Serialization.BytePatch;
using Zenject;

namespace Model.TestModel
{
	public partial class TestModelModel
	{
		private readonly SingleProperty<string> _stubbedStringField;

		[Inject]
		public TestModelModel(
			Id id,
			int intField,
			string explicitlyInitializedStringField,
			int param0,
			[InjectOptional] BytePatchSerializer<TestModelModel> serializer)
			: this(id, intField, explicitlyInitializedStringField, param0)
		{
			if (serializer?.PropertySerializers.Count == 0)
			{
				serializer.PropertySerializers.AddRange(new[]
				{
					new BytePatchPropertySerialization<TestModelModel>(
						nameof(IntField),
						(payload, target) => payload.WriteInt(target.IntField),
						(payload, target) => target.IntField = payload.ReadInt()),
					new BytePatchPropertySerialization<TestModelModel>(
						nameof(FloatField),
						(payload, target) => payload.WriteFloat(target.FloatField),
						(payload, target) => target.FloatField = payload.ReadFloat()),
					new BytePatchPropertySerialization<TestModelModel>(
						nameof(ExplicitlyInitializedStringField),
						(payload, target) => payload.WriteString(target.ExplicitlyInitializedStringField),
						(payload, target) => target.ExplicitlyInitializedStringField = payload.ReadString()),
					new BytePatchPropertySerialization<TestModelModel>(
						nameof(StubbedStringField),
						(payload, target) => payload.WriteString(target.StubbedStringField),
						(payload, target) => target.StubbedStringField = payload.ReadString()),
				});
			}
		}

		public string StubbedStringField
		{
			get => _stubbedStringField.Value;
			set => _stubbedStringField.Value = value;
		}
	}
}
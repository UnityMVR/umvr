using pindwin.umvr.Model;

namespace Model.TestModel
{
	public class TestModelMemo
	{
		public TestModelMemo(ITestModel model)
		{
			Id = model.Id;
			IntField = model.IntField;
			FloatField = model.FloatField;
			ExplicitlyInitializedStringField = model.ExplicitlyInitializedStringField;
			StubbedStringField = model.StubbedStringField;
		}
			
		public Id Id { get; }
		public int IntField { get; }
		public float FloatField { get; }
		public string ExplicitlyInitializedStringField { get; }
		public string StubbedStringField { get; }
	}
}
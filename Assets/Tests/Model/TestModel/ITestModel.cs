using pindwin.umvr.Attributes;
using pindwin.umvr.Model;

namespace Model.TestModel
{
	[AdditionalParameters(typeof(int))]
	public interface ITestModel : IModel
	{
		int IntField { get; }
		float FloatField { get; set; }
		[Initialization(InitializationLevel.Explicit)] string ExplicitlyInitializedStringField { get; set; }
		[CustomImplementation] string StubbedStringField { get; set; }
	}
}
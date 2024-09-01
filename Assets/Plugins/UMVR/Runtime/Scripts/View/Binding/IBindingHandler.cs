using pindwin.umvr.Command;

namespace pindwin.umvr.View.Binding
{
	public interface IBindingHandler : IValidatable<string>
	{
		void FeedToView<TPropertyType>(TPropertyType value);
		void FeedToView(string valueString);

		void FeedBackFromView(string newValue);
		void BindFeedback(IConditionalCommand<string> updateCommand);
		void Validate(string payload);
	}
}
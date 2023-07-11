using pindwin.development;
using pindwin.umvr.Command;
using pindwin.umvr.View.Widgets;

namespace pindwin.umvr.View.Binding
{
	public sealed class SinglePropertyBindingHandler : IBindingHandler
	{
		private IConditionalCommand<string> _updateMethod;
		private readonly ISinglePropertyWidget _displayTarget;

		public SinglePropertyBindingHandler(ISinglePropertyWidget displayTarget)
		{
			_displayTarget = displayTarget.AssertNotNull();
		}
		
		public void FeedToView<TPropertyType>(TPropertyType value)
		{
			string text = value?.ToString() ?? "null"; 
			FeedToView(text);
		}

		public void FeedToView(string valueString)
		{
			_displayTarget.Value = valueString;
		}

		public void FeedBackFromView(string newValue)
		{
			_updateMethod?.Execute(newValue);
		}

		public void BindFeedback(IConditionalCommand<string> updateCommand)
		{
			_updateMethod = updateCommand;
			if (_displayTarget is IPropertyWidget single)
			{
				single.IsReadonly = _updateMethod?.CanExecute() == true;
			}
		}

		public bool IsValid(string payload)
		{
			return _updateMethod?.IsValid(payload) == true;
		}

		public void Validate(string payload)
		{
			if (_displayTarget is TextFieldWidget concreteSingle)
			{
				concreteSingle.CanCommit = IsValid(payload);
			}
		}
	}
}
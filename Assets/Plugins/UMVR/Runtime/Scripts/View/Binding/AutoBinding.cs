using System;
using pindwin.development;
using pindwin.umvr.Command;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Binding
{
	public class AutoBinding : IDisposable
	{
		private readonly IBindingHandler _handler;
		private readonly Property _property;

		public AutoBinding(Property property, IBindingHandler widget, IConditionalCommand<string> feedback)
		{
			_property = property.AssertNotNull();
			_handler = widget.AssertNotNull();

			_property.Changed += RefreshProperty;
			RefreshProperty();

			_handler.BindFeedback(feedback);
		}

		public void Dispose()
		{
			if (_property != null)
			{
				_property.Changed -= RefreshProperty;
			}
		}

		private void RefreshProperty()
		{
			_handler.FeedToView(_property.ToString());
		}
	}
}
using pindwin.umvr.Model;
using pindwin.umvr.View.Binding;

namespace pindwin.umvr.View
{
	public interface IView
	{
		IBindingHandler Bind(IModel model, string label, BindingHandlerDescriptionFlags descriptionFlags = BindingHandlerDescriptionFlags.None);
	}
}
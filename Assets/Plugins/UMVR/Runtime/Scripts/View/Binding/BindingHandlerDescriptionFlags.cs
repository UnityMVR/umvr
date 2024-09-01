using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.View.Binding
{
	[Flags]
	public enum BindingHandlerDescriptionFlags
	{
		None = 0,
		Model = 1,
		Collection = 1 << 1
	}

	public static class BindingHandlerDescriptorExtensions
	{
		public static BindingHandlerDescriptionFlags GetDescriptor(this Property property)
		{
			var flags = BindingHandlerDescriptionFlags.None;
			if (property is CollectionProperty)
			{
				flags |= BindingHandlerDescriptionFlags.Collection;
			}

			if (typeof(IModel).IsAssignableFrom(property.Type))
			{
				flags |= BindingHandlerDescriptionFlags.Model;
			}

			return flags;
		}
	}
}
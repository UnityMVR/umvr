using System;

namespace pindwin.umvr.Attributes
{
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
	public sealed class AdditionalParameterAttribute : Attribute
	{
		public Type Type;
		public string Name;

		public AdditionalParameterAttribute(Type type, string name = null)
		{
			Type = type;
			Name = name;
		}
	}
}
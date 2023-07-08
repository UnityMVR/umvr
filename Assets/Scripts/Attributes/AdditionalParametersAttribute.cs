using System;

namespace pindwin.umvr.Attributes
{
	[AttributeUsage(AttributeTargets.Interface)]
	public sealed class AdditionalParametersAttribute : Attribute
	{
		public Type[] Types { get; }

		public AdditionalParametersAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}
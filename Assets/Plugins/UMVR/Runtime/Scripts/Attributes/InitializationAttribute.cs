using System;

namespace pindwin.umvr.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class InitializationAttribute : Attribute
	{
		public InitializationLevel Level { get; }

		public InitializationAttribute(InitializationLevel level = InitializationLevel.Default)
		{
			Level = level;
		}
	}
}
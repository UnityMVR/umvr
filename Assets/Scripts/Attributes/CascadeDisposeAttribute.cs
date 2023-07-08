using System;

namespace pindwin.umvr.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class CascadeDisposeAttribute : Attribute
	{
		public CascadeDirection Direction { get; }

		public CascadeDisposeAttribute(CascadeDirection direction = CascadeDirection.Downstream)
		{
			Direction = direction;
		}
	}
}
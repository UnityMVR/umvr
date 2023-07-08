using System;

namespace pindwin.umvr.Attributes
{
	[Flags]
	public enum CascadeDirection
	{
		None = 0,
		Downstream = 1,
		Upstream = 2,
		Both = Downstream | Upstream
	}
}
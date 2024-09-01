using System;

namespace pindwin.umvr.Model
{
	public interface INotifyChanged
	{
		event Action Changed;
	}
}
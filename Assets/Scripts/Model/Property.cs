using System;

namespace pindwin.umvr.Model
{
	public abstract class Property : IDisposable
	{
		public event Action Changed;

		protected void NotifyChanged()
		{
			Changed?.Invoke();
		}

		public abstract void Dispose();
	}
}
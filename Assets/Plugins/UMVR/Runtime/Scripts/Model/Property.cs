using System;

namespace pindwin.umvr.Model
{
	public abstract class Property : IDisposable
	{
		protected Property(string label, Type type)
		{
			Label = label;
			Type = type;
		}
		
		public event Action Changed;
		public string Label { get; }
		public Type Type { get; }

		protected void NotifyChanged()
		{
			Changed?.Invoke();
		}

		public abstract void Dispose();
	}
}
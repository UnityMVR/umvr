using System;

namespace pindwin.umvr.View.Widgets
{
	public interface IValidatedProperty
	{
		event Action<string> ValueProposed;
		bool CanCommit { get; set; }
	}
}
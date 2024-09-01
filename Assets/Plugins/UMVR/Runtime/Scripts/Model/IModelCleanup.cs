using System;

namespace pindwin.umvr.Model
{
	internal interface IModelCleanup
	{
		internal void AddCleanupHandler(Action<IModel> handler, int priority);
	}
}
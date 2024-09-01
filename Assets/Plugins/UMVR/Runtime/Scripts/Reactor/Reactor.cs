using System;
using pindwin.development;
using pindwin.umvr.Model;
using UniRx;

namespace pindwin.umvr.Reactor
{
	public abstract class Reactor<TModel> : IDisposable
		where TModel : Model<TModel>, IDisposable
	{
		private readonly TModel _model;
		protected CompositeDisposable Subscriptions { get; }
		protected Reactor(TModel model)
		{
			_model = model.AssertNotNull();
			_model.AddCleanupHandler(_ => Dispose(), CleanupPriority.Low);
			Subscriptions = new CompositeDisposable();
		}
		
		private bool _disposed;

		public void Dispose()
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;
			Subscriptions.Dispose();
			DisposeImpl();
		}

		public void BindDataSource()
		{
			BindDataSourceImpl(_model);
		}
		
		protected virtual void DisposeImpl() 
		{ }
		
		protected abstract void BindDataSourceImpl(TModel model);
	}
}
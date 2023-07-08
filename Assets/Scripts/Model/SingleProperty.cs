using System;
using UniRx;

namespace pindwin.umvr.Model
{
	public class SingleProperty<TItem> : Property, IObservable<TItem>
	{
		protected readonly ReactiveProperty<TItem> ValueStream;
		private readonly IDisposable _updateCallback;

		public SingleProperty()
		{
			ValueStream = new ReactiveProperty<TItem>();
			_updateCallback = ValueStream.Subscribe(_ => NotifyChanged());
		}

		public SingleProperty(TItem initialValue) :  this()
		{
			ValueStream.Value = initialValue;
		}

		public virtual TItem Value
		{
			get => ValueStream.Value;
			set => ValueStream.Value = value;
		}

		public IDisposable Subscribe(IObserver<TItem> observer)
		{
			return ValueStream.Subscribe(observer);
		}

		public override void Dispose()
		{
			_updateCallback?.Dispose();
			ValueStream?.Dispose();
		}

		public override string ToString()
		{
			return Value?.ToString() ?? "null";
		}
	}
}
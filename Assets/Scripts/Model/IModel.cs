using System;
using UniRx;

namespace pindwin.umvr.Model
{
	public interface IModel : IDisposable
	{
		event Action<IModel> Disposing;
		Id Id { get; }
		IObservable<TProperty> GetProperty<TProperty>(string label);
		ReactiveCollection<TItemType> GetCollection<TItemType>(string label);
		Property GetProperty(string label);
	}
}
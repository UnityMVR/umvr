using System;
using pindwin.umvr.Model;

namespace pindwin.umvr.Repository
{
	public interface ISecondaryIndex<TModel> : IDisposable
		where TModel : IModel
	{
		string PropertyName { get; }
		TModel Get(object id);
		void Add(TModel model);
		void Remove(TModel model);
	}
}
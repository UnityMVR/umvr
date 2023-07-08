using pindwin.umvr.Model;

namespace pindwin.umvr.Repository
{
	public interface ISingletonRepository<TModel> : IRepository<TModel>
		where TModel : IModel
	{
		TModel Value { get; }
	}
}
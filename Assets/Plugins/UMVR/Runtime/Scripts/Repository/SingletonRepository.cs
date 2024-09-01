using pindwin.umvr.Exceptions;
using pindwin.umvr.Model;
using pindwin.umvr.Reactor;

namespace pindwin.umvr.Repository
{
	public class SingletonRepository<TModel, TConcrete, TPresenter>
		: Repository<TModel, TConcrete, TPresenter>, ISingletonRepository<TModel>
		where TModel : IModel
		where TConcrete : Model<TConcrete>, TModel
		where TPresenter : Reactor<TConcrete>
	{
		protected SingletonRepository(ReactorFactory<TConcrete, TPresenter> presenterFactory)
			: base(presenterFactory)
		{
			Added += ValidateSingleton;
		}

		public TModel Value => ConcreteEntities.Count > 0 ? ConcreteEntities[0] : default;

		public static implicit operator TConcrete(SingletonRepository<TModel, TConcrete, TPresenter> r) =>
			r.ConcreteEntities.Count > 0 ? r.ConcreteEntities[0] : default;

		private void ValidateSingleton(TModel obj)
		{
			if (Entities.Count > 1)
			{
				while (Entities.Count > 1)
				{
					Remove(Entities[Entities.Count - 1]);
				}

				throw new UMVRException(
					$"Attempted to create multiple singletons of model type {typeof(TModel)} ({typeof(TConcrete)})."
				);
			}
		}
	}
}
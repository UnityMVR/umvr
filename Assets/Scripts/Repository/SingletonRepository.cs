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

		public TModel Value => Entities.Count > 0 ? Entities[0] : default;
	}
}
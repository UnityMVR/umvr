using pindwin.umvr.Model;
using Zenject;

namespace pindwin.umvr.Reactor
{
	public class ReactorFactory<TConcrete, TReactor> : PlaceholderFactory<TConcrete, TReactor>
		where TConcrete : Model<TConcrete>
		where TReactor : Reactor<TConcrete>
	{
		public override TReactor Create(TConcrete param)
		{
			TReactor product = base.Create(param);
			product.BindDataSource();
			return product;
		}
	}
}
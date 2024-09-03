// <auto-generated>
//	 This code was generated by a tool.
//
//	 Changes to this file may cause incorrect behavior and will be lost if
//	 the code is regenerated.
// </auto-generated>

using pindwin.umvr.Model;
using pindwin.umvr.Reactor;
using pindwin.umvr.Repository;
using pindwin.umvr.Serialization;
using Zenject;

namespace pindwin.umvr.example
{
	public partial class FooFactory : ModelFactory<IFoo, FooModel, System.String, pindwin.umvr.example.IFoo>
	{
		public FooFactory(IRepository<IFoo> repository, [InjectOptional] ISerializer<FooModel> serializer) : base(repository, serializer)
		{ }
	}

	public partial class FooRepository<TReactor> : Repository<IFoo, FooModel, TReactor> where TReactor : Reactor<FooModel>
	{
		public FooRepository(FooReactorFactory<TReactor> fooReactorFactory) : base(fooReactorFactory)
		{ }
	}
	
	public class FooReactorFactory<TReactor> : ReactorFactory<FooModel, TReactor> where TReactor : Reactor<FooModel>
	{ }

	internal class NullFooReactor : Reactor<FooModel>
	{
		public NullFooReactor(FooModel model) : base(model)
		{ }

		protected override void BindDataSourceImpl(FooModel model)
		{ }
	}
}

namespace pindwin.umvr.example.Generated
{
	public class FooInstallerBase : Installer<FooInstallerBase>
	{
		public static void Install(DiContainer container, bool useReactor = true)
		{
			container.Instantiate<FooInstallerBase>().InstallBindings(useReactor);
		}

		public override void InstallBindings()
		{
			InstallBindings(true);
		}

		public void InstallBindings(bool installReactor)
		{
			Container.BindFactory<Id, System.String, pindwin.umvr.example.IFoo, FooModel, FooFactory>();
			Container.BindInterfacesTo<FooFactory>().FromResolve();
			
			if (installReactor)
			{
				Container.BindFactory<FooModel, FooReactor, FooReactorFactory<FooReactor>>();
				Container.BindInterfacesAndSelfTo<FooRepository<FooReactor>>().AsSingle();
			}
			else
			{
				Container.BindFactory<FooModel, NullFooReactor, FooReactorFactory<NullFooReactor>>();
				Container.BindInterfacesAndSelfTo<FooRepository<NullFooReactor>>().AsSingle();
			}
		}
	}
}
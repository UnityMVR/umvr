using pindwin.umvr.example;
using pindwin.umvr.Example.Example.Scripts;
using pindwin.umvr.example.Generated;
using pindwin.umvr.Installation;
using pindwin.umvr.Model;
using pindwin.umvr.Repository;
using pindwin.umvr.View;
using pindwin.umvr.View.Binding;
using pindwin.umvr.View.Parsing;
using pindwin.umvr.View.Widgets;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;

public class ExampleInstaller : MonoInstaller
{
	[SerializeField] private DebuggerView _debuggerView;

	public override void InstallBindings()
	{
		FooInstallerBase.Install(Container);
		Container.BindInterfacesAndSelfTo<ExampleController>().AsSingle().NonLazy();

		Container.BindParserProviders();
		
		Container.BindSingleParser<IntPropertyParser>();
		Container.BindSingleParser<StringPropertyParser>();
		Container.BindSingleParser<FloatPropertyParser>();
		Container.BindSingleParser<BoolPropertyParser>();
		Container.BindSingleParser<ModelPropertyParser<IFoo>>();
		
		Container.BindCollectionParser<int>();
		Container.BindCollectionParser<IFoo>();
		
		Container.BindDebugger(_debuggerView.gameObject);
	}
}
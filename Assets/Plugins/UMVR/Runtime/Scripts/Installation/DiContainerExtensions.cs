using pindwin.umvr.Model;
using pindwin.umvr.View;
using pindwin.umvr.View.Binding;
using pindwin.umvr.View.Parsing;
using pindwin.umvr.View.Widgets;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace pindwin.umvr.Installation
{
	public static class DiContainerExtensions
	{
		public static void BindDebugger(this DiContainer Container, GameObject debuggerView)
		{
			Container.Bind<IView>().To<DebuggerView>().FromComponentInNewPrefab(debuggerView).AsSingle();
			Container.Bind<UIDocument>().FromComponentSibling().WhenInjectedInto<DebuggerView>();
			Container.BindFactory<Property, FeedValueBackCommand, FeedValueBackCommand.Factory>().AsSingle();
			Container.BindFactory<CollectionProperty, FeedCollectionEntryBackCommand, FeedCollectionEntryBackCommand.Factory>().AsSingle();

			Container.Bind<ModelDropdownLabelsProvider>().AsSingle();
			Container.Bind<GenericModelFactory>().AsSingle();
		}
		
		public static void BindSingleParser<TParser>(this DiContainer container)
			where TParser : PropertyParser
		{
			container.Bind<PropertyParser>().To<TParser>().AsSingle().WhenInjectedInto<PropertyParserProvider>();
		}

		public static void BindCollectionParser<TCollectedType>(this DiContainer container)
		{
			container.Bind<PropertyParser>().To<CollectionPropertyParser<TCollectedType>>().AsSingle()
				.WhenInjectedInto<CollectionParserProvider>();
		}

		public static void BindParserProviders(this DiContainer container)
		{
			container.BindInterfacesAndSelfTo<PropertyParserProvider>().AsSingle();
			container.BindInterfacesAndSelfTo<CollectionParserProvider>().AsSingle();
		}
	}
}
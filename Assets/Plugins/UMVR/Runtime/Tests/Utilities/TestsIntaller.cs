using System;
using Zenject;

namespace Tests.Utilities
{
	public class TestsInstaller : Installer
	{
		public override void InstallBindings()
		{ }

		public static void SetUp(
			out DiContainer container,
			out TestsInstaller installer,
			Action<DiContainer> additionalBindingRoutine)
		{
			container = new DiContainer();
			installer = container.Instantiate<TestsInstaller>();
			installer.InstallBindings();
			additionalBindingRoutine?.Invoke(container);
		}

		public static void TearDown(ref DiContainer container, ref TestsInstaller installer)
		{
			container.UnbindAll();
			container = null;
			installer = null;
		}
	}
}
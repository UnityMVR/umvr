using System;

namespace pindwin.umvr.Editor.Generation
{
	//todo this should probably be removed
	[Flags]
	public enum GenerationFilter
	{
		Model = 1,
		ReactorBase = 1 << 1,
		ReactorStub = 1 << 2,
		Installer = 1 << 3,
		ModelStub = 1 << 4,
		FactoryStub = 1 << 5,
		RepositoryStub = 1 << 6
	}
}
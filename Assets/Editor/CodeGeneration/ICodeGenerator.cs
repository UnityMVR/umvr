using GenerationParams;

namespace pindwin.umvr.Editor.CodeGeneration
{
	//todo add async version
	public interface ICodeGenerator
	{
		IGenerationProcessedResult Run(ICodeGeneratorSession session);
	}
}
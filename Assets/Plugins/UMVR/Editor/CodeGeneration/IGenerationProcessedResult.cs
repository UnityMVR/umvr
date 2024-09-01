using System.Collections.Generic;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public interface IGenerationProcessedResult
	{
		IReadOnlyList<IFileGenerationResult> ProcessedFiles { get; }
	}
}
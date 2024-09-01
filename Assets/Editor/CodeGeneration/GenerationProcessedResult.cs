using System.Collections.Generic;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public class GenerationProcessedResult : IGenerationProcessedResult
	{
		private readonly List<IFileGenerationResult> _processedFiles;

		public GenerationProcessedResult()
		{
			_processedFiles = new List<IFileGenerationResult>();
		}

		public IReadOnlyList<IFileGenerationResult> ProcessedFiles => _processedFiles;

		public void Add(IFileGenerationResult result)
		{
			_processedFiles.Add(result);
		}
	}
}
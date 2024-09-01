using System.Collections.Generic;
using pindwin.umvr.Editor.Generation;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public interface IFileGenerationResult
	{
		FileGenerationRequest Request { get; }
		GenerationResultInfo ResultInfo { get; }
		public IReadOnlyList<string> Errors { get; }
		public bool HasErrors { get; }
		public string Path { get; }
		public string Text { get; }
	}
}
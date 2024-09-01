using System.Collections.Generic;
using GenerationParams;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public interface ICodeGeneratorSession
	{
		GenSettings GenSettings { get; }
		IReadOnlyDictionary<string, FileGenerationRequest> Requests { get; }
	}
}
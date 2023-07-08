using System.Collections.Generic;
using GenerationParams;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public class CodeGeneratorSession : ICodeGeneratorSession
	{
		public CodeGeneratorSession(string rootPath, IReadOnlyDictionary<string, FileGenerationRequest> requests, GenSettings settings)
		{
			Requests = requests;
			GenSettings = settings;
		}

		public GenSettings GenSettings { get; }
		public IReadOnlyDictionary<string, FileGenerationRequest> Requests { get; }
	}
}
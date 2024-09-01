using System.Collections.Generic;
using pindwin.umvr.Editor.Generation;
using UnityEditor.PackageManager.Requests;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public class FileGenerationResult : IFileGenerationResult
	{
		private readonly List<string> _errors;

		public FileGenerationResult(FileGenerationRequest request)
		{
			Request = request;
			_errors = new List<string>();
		}

		public IReadOnlyList<string> Errors => _errors;

		public bool HasErrors => Errors.Count > 0;
		public string Path => Request.OutputPath;
		public FileGenerationRequest Request { get; }
		public GenerationResultInfo ResultInfo { get; private set; }
		public string Text { get; set; }

		public void AddError(string error)
		{
			_errors.Add(error);
		}

		public bool WasSuccess
		{
			set => ResultInfo = value ? GenerationResultInfo.Success : GenerationResultInfo.Failure;
		}
	}
}
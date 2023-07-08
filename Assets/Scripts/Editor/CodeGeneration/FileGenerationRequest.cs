using System.Text;
using pindwin.development;
using UnityEngine;
using ILogger = pindwin.development.ILogger;

namespace pindwin.umvr.Editor.CodeGeneration
{
	public class FileGenerationRequest : ScriptableObject, ILogger
	{
		public string Name;
		public bool IsRequested;
		public string Messages;
		
		[SerializeField] private bool _isEssential;
		[SerializeField] private string _inputPath;
		[SerializeField] private string _ensurePath;
		[SerializeField] private string _outputPath;
		
		private readonly StringBuilder _sb = new();

		public bool IsEssential
		{
			get => _isEssential;
			set => _isEssential = value;
		}

		public string InputPath
		{
			get => _inputPath;
			set => _inputPath = value;
		}

		public string EnsurePath
		{
			get => _ensurePath;
			set => _ensurePath = value;
		}

		public string OutputPath
		{
			get => _outputPath;
			set => _outputPath = value;
		}

		public void Log(string message, LogSeverity severity = LogSeverity.Debug)
		{
			_sb.AppendLine($"{severity.ToString().ToUpper()}: {message}");
			Messages = _sb.ToString();
		}
	}
}
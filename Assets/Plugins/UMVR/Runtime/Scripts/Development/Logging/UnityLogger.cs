using UnityEngine;

namespace pindwin.development
{
	public class UnityLogger : ILogger
	{
		public void Log(string message, LogSeverity severity = LogSeverity.Debug)
		{
			switch (severity)
			{
				case LogSeverity.Warning:
					Debug.LogWarning(message);
					break;
				case LogSeverity.Error:
					Debug.LogError(message);
					break;
				case LogSeverity.Debug:
				case LogSeverity.Notice:
				default:
					Debug.Log(message);
					break;
			}
		}
	}
}
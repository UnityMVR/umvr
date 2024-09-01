namespace pindwin.development
{
	public class NullLogger : ILogger
	{
		public void Log(string message, LogSeverity severity = LogSeverity.Debug) { }
	}
}
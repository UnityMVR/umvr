namespace pindwin.development
{
	public interface ILogger
	{
		void Log(string message, LogSeverity severity = LogSeverity.Debug);
	}
}
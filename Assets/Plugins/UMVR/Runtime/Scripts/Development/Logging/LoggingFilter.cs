namespace pindwin.development
{
	public class LoggingFilter : ILogger
	{
		private readonly LogSeverity _minimumLogLevel;
		private readonly ILogger _decoratedLogger;
		
		public LoggingFilter(ILogger decoratedLogger, LogSeverity minimum)
		{
			_decoratedLogger = decoratedLogger.AssertNotNull();
			_minimumLogLevel = minimum;
		}

		public void Log(string message, LogSeverity severity = LogSeverity.Debug)
		{
			if (severity < _minimumLogLevel)
			{
				return;
			}
			
			_decoratedLogger.Log(message, severity);
		}
	}
}
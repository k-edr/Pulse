namespace IngameScript.Pulse.Logging
{
    public abstract class BaseLogger : ILoggable
    {
        public readonly LogLevel LoggerLevel;

        protected BaseLogger(LogLevel loggerLevel)
        {
            LoggerLevel = loggerLevel;
        }

        public abstract void Log(string message, LogLevel logLevel = LogLevel.Any);

        public abstract void LogLine(string message, LogLevel logLevel = LogLevel.Any);

        protected bool RightLogLevel(LogLevel loglevel) => (LoggerLevel & loglevel) != 0;
    }
}
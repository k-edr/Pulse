namespace IngameScript.Pulse.Logging
{
    public interface ILoggable
    {
        void Log(string message, LogLevel logLevel = LogLevel.Any);

        void LogLine(string message, LogLevel logLevel = LogLevel.Any);
    }
}
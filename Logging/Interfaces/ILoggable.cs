using IngameScript.Pulse.Logging.Enums;

namespace IngameScript.Pulse.Logging.Interfaces
{
    public interface ILoggable
    {
        void Log(string message, LogLevel logLevel = LogLevel.Any);

        void LogLine(string message, LogLevel logLevel = LogLevel.Any);
    }
}
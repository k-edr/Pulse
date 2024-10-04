using IngameScript.Pulse.Logging.Bases;
using IngameScript.Pulse.Logging.Enums;
using System;
namespace IngameScript.Pulse.Logging.Services
{
    public class EchoLogger : BaseLogger
    {
        Action<string> _echo;

        public EchoLogger(Action<string> echo, LogLevel loggerLevel = LogLevel.Any):base(loggerLevel) 
        {
            _echo = echo;
        }

        public override void Log(string message, LogLevel logLevel = LogLevel.Any)
        {
            if (RightLogLevel(logLevel))
            {
                _echo(message);
            }
        }

        public override void LogLine(string message, LogLevel logLevel = LogLevel.Any)
            => Log("\n" + message, logLevel);
    }
}
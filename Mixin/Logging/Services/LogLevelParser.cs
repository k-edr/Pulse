using IngameScript.Pulse.Logging.Enums;
using System;

namespace IngameScript.Pulse.Logging.Services
{
    public static class LogLevelParser
    {
        public static LogLevel Parse(string str)
        {
            switch(str) 
            {
                case nameof(LogLevel.None): return LogLevel.None;
                case nameof(LogLevel.Debug): return LogLevel.Debug;
                case nameof(LogLevel.Runtime): return LogLevel.Runtime;
                case nameof(LogLevel.Any): return LogLevel.Any;
                default: throw new ArgumentException("Undefined LogLevel", nameof(str));
            }
        }
    }
}

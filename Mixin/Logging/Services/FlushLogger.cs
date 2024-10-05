using IngameScript.Pulse.Logging.Bases;
using IngameScript.Pulse.Logging.Enums;
using IngameScript.Pulse.Logging.Interfaces;
using System.Text;
namespace IngameScript.Pulse.Logging.Services
{
    public class FlushLogger : BaseLogger, IFlushable
    {
        private ILoggable _logger;

        private StringBuilder _stringBuilder;

        public FlushLogger(ILoggable logger):base((logger as BaseLogger).LoggerLevel)
        {
            _logger = logger;

            _stringBuilder = new StringBuilder();
        }

        public void Flush()
        {
            _logger.Log(_stringBuilder.ToString());

            _stringBuilder.Clear();
        }

        public override void Log(string message, LogLevel logLevel = LogLevel.Any)
        {
            if(RightLogLevel(logLevel))
            {
                _stringBuilder.Append(message);
            }
        }

        public override void LogLine(string message, LogLevel logLevel = LogLevel.Any)
        => Log("\n" + message, logLevel);
    }
}
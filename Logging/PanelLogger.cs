using Sandbox.ModAPI.Ingame;
namespace IngameScript.Pulse.Logging
{
    public class PanelLogger : BaseLogger
    {
        private IMyTextPanel _textPanel;

        public PanelLogger(IMyTextPanel textPanel, LogLevel loggerLevel = LogLevel.Any): base(loggerLevel)
        {
            _textPanel = textPanel;          
        }

        public override void Log(string message, LogLevel logLevel = LogLevel.Any)
        {
            if(RightLogLevel(logLevel))
            {
                _textPanel.WriteText(message, true);
            }
        }

        public override void LogLine(string message, LogLevel logLevel = LogLevel.Any)
        => Log("\n" + message, logLevel);
    }
}
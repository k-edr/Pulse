using IngameScript.Pulse.CommandInteface;
using IngameScript.Pulse.EnvironmentVariables;
using IngameScript.Pulse.Logging;
using IngameScript.Pulse.Logging.Interfaces;
using IngameScript.Pulse.Logging.Services;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static VRage.Game.VisualScripting.ScriptBuilder.MyVSAssemblyProvider;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        protected IMyIni Environment;

        protected ILoggable Logger;

        protected CommandRegister Register;

        protected IDataProvider ArgumentProvider;

        public Program()
        {
            Environment = new MyIniConfigParser(Me);

            Logger = new FlushLogger(
                new PanelLogger(
                    loggerLevel: LogLevelParser.Parse(Environment.Get("Grid", "LoggingLevel").ToString()),
                    textPanel: GridTerminalSystem.GetBlockWithName(
                        Environment.Get("Grid", "LoggerPanel").ToString()) as IMyTextPanel));

            Register = CommandRegister.Register;

            ArgumentProvider = new ArgumentProvider();

            Init();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            ExecuteCommand(argument, updateSource);
            ExecuteCycle(argument, updateSource);
        }

        private void ExecuteCommand(string argument,UpdateType updateSource)
        {
            if (((updateSource & (UpdateType.Terminal | UpdateType.Trigger)) != 0) &&
                argument != string.Empty)
            {
                ArgumentProvider = new ArgumentProvider(argument);

                Action action;

                if (Register.TryGet(argument, out action))
                {
                    action.Invoke();
                }

                return;
            }
        }

    }
}

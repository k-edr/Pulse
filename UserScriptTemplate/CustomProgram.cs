using IngameScript.Pulse.CommandInteface;
using IngameScript.Pulse.ContiniousExecution.Interfaces;
using IngameScript.Pulse.ContiniousExecution.Services;
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

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        public IMyIni Environment;

        public ILoggable Logger;

        public CommandRegister Register;

        public IDataProvider ArgumentProvider;

        public readonly IContiniousExecutor ContiniousExecutor;

        public Program()
        {
            Environment = new MyIniConfigParser(Me);

            Logger = new FlushLogger(
                new PanelLogger(
                    loggerLevel: LogLevelParser.Parse(Environment.Get("Grid", "LoggingLevel").ToString()),
                    textPanel: GridTerminalSystem.GetBlockWithName(
                        Environment.Get("Grid", "LoggerPanel").ToString()) as IMyTextPanel));

            Register = CommandRegister.Register;

            ContiniousExecutor = new OneTaskPerCycleExexutor(Logger);

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

                char splitter = (char)Environment.Get("Grid", "CommandSplitter").ToInt32();

                string command = argument;

                Action action;

                if (argument.Contains(splitter))
                {
                    command = argument.Split(splitter)[0];
                }

                if (Register.TryGet(command, out action))
                {
                    action.Invoke();
                }

            }
        }

    }
}

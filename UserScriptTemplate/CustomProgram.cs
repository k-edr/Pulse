using IngameScript.Pulse.EnvironmentVariables;
using IngameScript.Pulse.Logging;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        protected IMyIni Environment;

        protected ILoggable Logger;

        public Program()
        {
            Environment = new MyIniConfigParser(Me);

            Logger = new FlushLogger(
                new PanelLogger(
                    GridTerminalSystem.GetBlockWithName(
                        Environment.Get("Grid", "LoggerPanel").ToString()) as IMyTextPanel));

            Init();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Execute(argument, updateSource);
        }

    }
}

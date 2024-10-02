using IngameScript.Pulse.EnvironmentVariables;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public readonly IEnvironment Environment;

        public Program()
        {
            Environment = new MyIniConfigParser(Me);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Echo(Environment.Get("Grid;Hello"));

            var ini = Environment as IMyIni;

            if(ini != null)
            {
                var a = ini.Get("Grid", "A").ToInt32();
                var b = ini.Get("Grid", "B").ToInt32();

                Echo($"\n Sum {a + b}");
            }         
        }
    }
}

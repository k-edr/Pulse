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
        public Program()
        {
            Init();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Execute(argument, updateSource);
        }

    }
}

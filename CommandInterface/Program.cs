using IngameScript.Pulse.CommandInteface;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        CommandRegister Register;

        IDataProvider ArgumentProvider;

        public Program()
        {
            Register = CommandRegister.Register;

            Register.TryAdd("Hello", () => Echo("Hello World!"));
            Register.TryAdd("HelloName", (str) => Echo($"Hello {str.Get.Split(';')[1]}"), ArgumentProvider);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            ArgumentProvider = new ArgumentProvider(argument);

            Action action;

            Register.TryGet(argument,out action);

            action.Invoke();
        }
    }
}

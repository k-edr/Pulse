using IngameScript.Pulse.Logging;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        ILoggable EchoLogger;

        ILoggable PanelLogger;

        ILoggable FlushEchoLogger;

        ILoggable FlushPanelLogger;

        public Program()
        {
            EchoLogger = new EchoLogger(Echo);
            PanelLogger = new PanelLogger(GridTerminalSystem.GetBlockWithName("Text Panel") as IMyTextPanel);
            FlushEchoLogger = new FlushLogger(EchoLogger);
            FlushPanelLogger = new FlushLogger(PanelLogger);
        }

        public void Main(string argument, UpdateType updateSource)
        {
            EchoLogger.LogLine($"Echo logger. {DateTime.Now:T}");

            PanelLogger.LogLine($"Panel logger. {DateTime.Now:T}");

            for (int i = 0; i < 10; i++)
            {
                FlushEchoLogger.LogLine($"FlushEchoLogger {i}. {DateTime.Now:T}");
                FlushPanelLogger.LogLine($"FlushPanelLogger {i}. {DateTime.Now:T}");
            }

            (FlushEchoLogger as IFlushable).Flush();

            (FlushPanelLogger as IFlushable).Flush();
        }    
    }
}
using IngameScript.Pulse.ContiniousExecution;
using IngameScript.Pulse.ContiniousExecution.Interfaces;
using IngameScript.Pulse.ContiniousExecution.Models;
using IngameScript.Pulse.ContiniousExecution.Services;
using IngameScript.Pulse.Logging.Enums;
using IngameScript.Pulse.Logging.Interfaces;
using IngameScript.Pulse.Logging.Services;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public readonly IContiniousExecutor Executor;

        public readonly ILoggable Logger;

        public Program()
        {
            Logger = new FlushLogger(new EchoLogger(Echo));

            Executor = new OneTaskPerCycleExexutor(Logger);

            Executor.Add(new ContiniousTask(Function));
            Executor.Add(new ContiniousTask(Function));
            Executor.Add(new ContiniousTask(Function));
        }  

        public IEnumerable<bool> Function()
        {
            var arr = GenerateArr();
            yield return false;

            IncreaseOneArr(arr);
            yield return false;

            IncreaseOneArr(arr);
            yield return false;

            IncreaseOneArr(arr);
            yield return true;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            Executor.Execute();

            if (argument.Equals("ShowLogs"))
            {
                (Logger as IFlushable).Flush();
            }
        }     

        int[] IncreaseOneArr(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i]= arr[i] + 1;
            }

            Logger.LogLine($"IncreaseOneArr {arr[0]}", LogLevel.Debug);

            return arr;
        }

        int[] GenerateArr()
        {
            Logger.LogLine($"Memory inited", LogLevel.Debug);

            return new int[10000];
        }
    }
}
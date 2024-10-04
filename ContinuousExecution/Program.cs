using IngameScript.Pulse.ContiniousExecution;
using IngameScript.Pulse.Logging.Interfaces;
using IngameScript.Pulse.Logging.Services;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
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
        ContiniousExecutor Executor;

        ILoggable Logger;
        public Program()
        {
            Logger = new FlushLogger(new EchoLogger(Echo));

            Executor = new ContiniousExecutor(Logger);

            Executor.Add(new ContiniousTask(GenerateArr, IncreaseOneArr, IncreaseOneArr, IncreaseOneArr));
            Executor.Add(new ContiniousTask(GenerateArr, IncreaseOneArr, IncreaseOneArr, IncreaseOneArr));
            Executor.Add(new ContiniousTask(GenerateArr, IncreaseOneArr, IncreaseOneArr, IncreaseOneArr));

        }

        public void Main(string argument, UpdateType updateSource)
        {           
            long i = 1;
            while (true)
            {
                Logger.LogLine("Program Tick: " + i++ + " ");
                Executor.Execute(new NTicksTimeProvider(10));

                if (i == 10) break;
            }

            Logger.Log("");
        }

        object IncreaseOneArr(object obj)
        {
            int[] arr = (int[])obj;

            for (int i = 1; i < arr.Length; i++)
            {
                arr[i-1]= i*5;
            }

            return arr;
        }

        object GenerateArr(object obj)
        {
            return new int[10000];
        }
    }
}

namespace IngameScript.Pulse.ContiniousExecution
{

    class MaxPossibleTimeProvider : IAvailableTimeProvider
    {
        public TimeSpan Get()
        {
            return TimeSpan.MaxValue;
        }
    }
    class NTicksTimeProvider : IAvailableTimeProvider
    {
        private int _ticks;
        public NTicksTimeProvider(int ticks)
        {
            _ticks = ticks;
        }

        public TimeSpan Get()
        {
            return TimeSpan.FromTicks(_ticks);
        }
    }

    interface IAvailableTimeProvider
    {
        TimeSpan Get();
    }

    public enum TaskExecutionStatus
    {
        None = 0,
        NotExecuted = 1,
        NotFullyExecuted = 2,
        FullyExecuted = 4
    }

    class ContiniousTask
    {
        private Func<object, object>[] _subTasks;

        private object _lastReturnedValue = null;

        public int TaskIndex { get; private set; }

        public int TotalSubTasks => _subTasks.Length;

        public TaskExecutionStatus TaskExecutionStatus { get; private set; } = TaskExecutionStatus.NotExecuted;

        public ContiniousTask(params Func<object, object>[] subTasks)
        {
            _subTasks = subTasks;
        }

        public TaskExecutionStatus ExecuteNext()
        {
            if (TaskIndex < _subTasks.Length)
            {
                _lastReturnedValue = _subTasks[TaskIndex++].Invoke(_lastReturnedValue);

                TaskExecutionStatus = TaskExecutionStatus.NotFullyExecuted;
            }

            if (TaskIndex >= _subTasks.Length)
            {
                TaskExecutionStatus = TaskExecutionStatus.FullyExecuted;
            }

            return TaskExecutionStatus;
        }
    }

    class ContiniousExecutor
    {
        private List<ContiniousTask> _tasks = new List<ContiniousTask>();

        private ILoggable _logger;
        public ContiniousExecutor(ILoggable logger)
        {
            _logger = logger;
        }

        public void Add(ContiniousTask task)
            => _tasks.Add(task);

        public bool Remove(ContiniousTask task)
            => _tasks.Remove(task);

        public bool Contains(ContiniousTask task)
            => _tasks.Contains(task);

        public void Execute(IAvailableTimeProvider availableTime)
        {
            var executedTasksToRemove = new List<ContiniousTask>();

            var availableTicks = availableTime.Get().Ticks;
            long usedTicks = 0;

            foreach (var task in _tasks)
            {
                if (usedTicks >= availableTicks)
                    break;

                if (task.TaskExecutionStatus == TaskExecutionStatus.NotExecuted)
                {
                    _logger.LogLine("Start task executing " + task.GetHashCode());
                }
                else if (task.TaskExecutionStatus == TaskExecutionStatus.NotFullyExecuted)
                {
                    _logger.LogLine("Continue task execution ");
                }

                while (usedTicks < availableTicks && task.TaskExecutionStatus != TaskExecutionStatus.FullyExecuted)
                {
                    var start = DateTime.Now;
                    task.ExecuteNext();
                    var end = DateTime.Now;
                    //usedTicks += (end - start).Ticks; // Simulate that each task execution consumes 1 tick
                    usedTicks += 1;
                }

                if (task.TaskExecutionStatus == TaskExecutionStatus.FullyExecuted)
                {
                    executedTasksToRemove.Add(task);
                    _logger.LogLine("Finished task execution " + task.GetHashCode());
                }
            }

            executedTasksToRemove.ForEach(t => _tasks.Remove(t));
        }

    }
}
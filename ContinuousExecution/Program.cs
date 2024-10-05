using IngameScript.Pulse.ContiniousExecution;
using IngameScript.Pulse.Logging.Enums;
using IngameScript.Pulse.Logging.Interfaces;
using IngameScript.Pulse.Logging.Services;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;


#pragma warning disable ProhibitedMemberRule // Prohibited Type Or Member


namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        ContiniousExecutor Executor;

        ILoggable Logger;

        public Program()
        {
            Logger = new FlushLogger(new EchoLogger(ConsoleEcho));

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
                Logger.LogLine($"Program Tick: {i++} ", LogLevel.Debug);
                Executor.Execute(new NTicksTimeProvider(1));//for 1 task per tick

                if (i == 100) break;
            }

            (Logger as IFlushable).Flush();

            Console.ReadLine();
        }

        public void ConsoleEcho(string str)
        {
            Console.Write(str);
        }

        object IncreaseOneArr(object obj)
        {
            int[] arr = (int[])obj;

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i]= arr[i] + 1;
            }

            Logger.LogLine($"IncreaseOneArr {arr[0]}", LogLevel.Debug);

            return arr;
        }

        object GenerateArr(object obj)
        {
            Logger.LogLine($"Memory inited", LogLevel.Debug);
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
            if (TaskIndex <= _subTasks.Length)
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
            long availableTicks = availableTime.Get().Ticks;
            long usedTicks = 0;

            if (_tasks.Count == 0)
                return;

            foreach (var task in _tasks)
            {
                while(usedTicks < availableTicks)
                {
                    var beginIteration = DateTime.Now.Ticks;

                    if (task.TaskExecutionStatus == TaskExecutionStatus.FullyExecuted)
                        break;
                    else
                    {
                        _logger.LogLine($"Execute task: {task.GetHashCode()} ", LogLevel.Debug);

                        task.ExecuteNext();
                    }

                    var endIteration = DateTime.Now.Ticks;

                    usedTicks += endIteration - beginIteration;
                }
            }

            _tasks.RemoveAll(t => t.TaskExecutionStatus == TaskExecutionStatus.FullyExecuted);
        }

    }
}
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
        public Program()
        {
            
        }
     
        public void Main(string argument, UpdateType updateSource)
        {
           
        }
    }
}

namespace IngameScript.Pulse.ContinuousExecution
{
    class Task
    {
        private Action _action;

        public SubTask Next { get; } = null;

        public bool Executed { get; private set; } = false;

        public void Run()
        {
            _action.Invoke();
        }

        public Task(Action action)
        {
            _action = action;
        }

        public Task(Action action, IEnumerable<Func<object, object[]>> subTasks) : this(action)
        {
            var enumerator = subTasks.GetEnumerator();

            Next = new SubTask(enumerator.Current);

            var temp = Next;
            while (enumerator.MoveNext())
            {
                temp.Next = new SubTask(enumerator.Current);

                temp = temp.Next;
            }
            
        }
    }

    class SubTask
    {
        private Func<object, object[]> _func;

        public SubTask Next = null;

        public bool Executed { get; private set; } = false;

        public object Run(object[] args = null)
        {
            var result = _func.Invoke(args);

            Executed = true;

            return result;
        }

        public SubTask(Func<object, object[]> func) 
        {
            _func = func;
        }

        public SubTask(Func<object, object[]> func, SubTask next) : this(func)
        {
            Next = next;
        }
    }

    class Executor
    {
        private List<Task> _tasks = new List<Task>();

        public void Run(long availableTicks = -1)
        {
            long usedTicks = 0;
            bool hasTimeLimit = availableTicks >= 0;

            foreach (var task in _tasks)
            {
                if (!task.Executed)
                {
                    if (!hasTimeLimit || usedTicks < availableTicks)
                    {
                        usedTicks += ExecuteTask(task, ref usedTicks, availableTicks, hasTimeLimit);
                    }
                }

                var subTask = task.Next;
                while (subTask != null && (!hasTimeLimit || usedTicks < availableTicks))
                {
                    if (!subTask.Executed)
                    {
                        usedTicks += ExecuteSubTask(subTask, ref usedTicks, availableTicks, hasTimeLimit);
                    }
                    subTask = subTask.Next;
                }
            }
        }

        private long ExecuteSubTask(SubTask task, ref long usedTicks, long availableTicks, bool hasTimeLimit)
        {
            var start = DateTime.Now;

            task.Run();

            var end = DateTime.Now;
            return (end - start).Ticks;
        }

        private long ExecuteTask(Task task, ref long usedTicks, long availableTicks, bool hasTimeLimit)
        {
            var start = DateTime.Now;

            task.Run();

            var end = DateTime.Now;
            return (end - start).Ticks;
        }


        public void Add(Task task)
        {
            _tasks.Add(task);
        }

        public void Remove(Task task)
        {
            _tasks.Remove(task);
        }
    }
}
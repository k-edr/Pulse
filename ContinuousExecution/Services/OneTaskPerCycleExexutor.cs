using IngameScript.Pulse.ContiniousExecution.Interfaces;
using IngameScript.Pulse.ContiniousExecution.Models;
using IngameScript.Pulse.Logging.Interfaces;
using System.Collections.Generic;

namespace IngameScript.Pulse.ContiniousExecution.Services
{
    public class OneTaskPerCycleExexutor : IContiniousExecutor
    {
        private List<ContiniousTask> _tasks = new List<ContiniousTask>();

        private ILoggable _logger;

        public OneTaskPerCycleExexutor(ILoggable logger)
        {
            _logger = logger;
        }

        public void Add(ContiniousTask task)
        {
            _tasks.Add(task);
        }

        public bool Remove(ContiniousTask task)
        {
            return _tasks.Remove(task);
        }

        public bool Contains(ContiniousTask task)
        {
            return _tasks.Contains(task);
        }

        public virtual void Execute()
        {
            if (_tasks.Count == 0)
            { 
                return;
            }

            ContiniousTask toRemove = null;

            foreach (var task in _tasks)
            {
                _logger.LogLine($"Execute task: {task.GetHashCode()}");

                if (!task.GetEnumerator().MoveNext())
                {
                    toRemove = task;

                    _logger.LogLine($"Execute task finished: {task.GetHashCode()}");
                }

                break;
            }

            if (toRemove != null)
            {
                _tasks.Remove(toRemove);
            }
        }
    }
}
using IngameScript.Pulse.ContiniousExecution.Models;

namespace IngameScript.Pulse.ContiniousExecution.Interfaces
{
    public interface IContiniousExecutor
    {
        void Execute();

        void Add(ContiniousTask task);

        bool Remove(ContiniousTask task);

        bool Contains(ContiniousTask task);
    }
}
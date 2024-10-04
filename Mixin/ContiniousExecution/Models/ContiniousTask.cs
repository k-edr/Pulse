using System;
using System.Collections;
using System.Collections.Generic;

namespace IngameScript.Pulse.ContiniousExecution.Models
{
    public class ContiniousTask : IEnumerable<bool>
    {
        private Func<IEnumerable<bool>> _func;

        private IEnumerator<bool> _enumerator;

        public ContiniousTask(Func<IEnumerable<bool>> task)
        {
            _func = task;
        }

        public IEnumerator<bool> GetEnumerator()
        {
            if (_enumerator == null)
            {
                return _enumerator = _func.Invoke().GetEnumerator();
            }
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
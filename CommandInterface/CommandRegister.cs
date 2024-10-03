using System;
using System.Collections.Generic;

namespace IngameScript.Pulse.CommandInteface
{
    public class CommandRegister
    {
        private Dictionary<string, Action> _commands;

        private static CommandRegister _instance;

        public static CommandRegister Register 
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new CommandRegister();
                }

                return _instance;
            }
        }

        private CommandRegister()
        {
            _commands = new Dictionary<string, Action>();

        }
       
        public void TryGet(string command, out Action action)
        {
            _commands.TryGetValue(command, out action);
        }

        public bool TryAdd(string command, Action<IDataProvider> action, IDataProvider provider)
        {
            return TryAdd(command, () => action.Invoke(provider));
        }


        public bool TryAdd(string command, Action action)
        {
            if (_commands.ContainsKey(command))
            {
                return false;
            }
            else
            {
                _commands.Add(command, action);

                return true;
            }
        }

        public bool TryRemove(string command)
        {
            if (_commands.ContainsKey(command))
            {
                _commands.Remove(command);

                return true;
            }

            return false;
        }
    }
}

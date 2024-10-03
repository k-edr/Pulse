using System;
using System.Collections.Generic;

namespace IngameScript.Pulse.CommandInteface
{
    public interface IDataProvider
    {
        string Get { get; }

        bool IsEmpty { get; }
    }

    struct ArgumentProvider : IDataProvider
    {
        public string _data;

        private bool _isEmpty;

        public ArgumentProvider(string data)
        {
            _data = data;

            if(_data == string.Empty)
            {
                _isEmpty = true;
            }

            _isEmpty = false;
        }

        public bool IsEmpty => _isEmpty;
        
        public string Get => _data;
    }

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

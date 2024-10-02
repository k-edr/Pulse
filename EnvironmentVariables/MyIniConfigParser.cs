using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript.Pulse.EnvironmentVariables
{
    public class MyIniConfigParser : IMyIni
    {
        private readonly string _config;

        private readonly MyIni _myIni; 

        public MyIniConfigParser(IMyTerminalBlock block)
        {
            _config = block.CustomData;

            _myIni = new MyIni();

            if (!_myIni.TryParse(_config))
            {
                throw new ArgumentException("Config isn't MyIni", $"{nameof(block)} {nameof(_config)}");
            }
        }

        public MyIniValue Get(string sector, string name)
        {
           return _myIni.Get(sector, name);
        }

        public string Get(string name, char separator = ';')
        {
            var arr = name.Split(separator);

            return Get(arr[0], arr[1]).ToString();
        }

        public bool TryGet(string sector, string name, out MyIniValue value)
        {
            value = _myIni.Get(sector, name);

            return value.IsEmpty;
        }
    }
}

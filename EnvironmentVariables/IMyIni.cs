using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript.Pulse.EnvironmentVariables
{
    public interface IMyIni:IEnvironment
    {
        MyIniValue Get(string sector, string name);

        bool TryGet(string sector, string name, out MyIniValue value);
    }
}

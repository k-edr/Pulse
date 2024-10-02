using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public interface ITemplateProgram
    {
        void Execute(string argument, UpdateType updateSource);

        void Init();
    }
}

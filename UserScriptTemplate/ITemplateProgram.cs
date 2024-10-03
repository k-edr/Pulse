using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public interface ITemplateProgram
    {
        void ExecuteCycle(string argument, UpdateType updateSource);

        void Init();
    }
}

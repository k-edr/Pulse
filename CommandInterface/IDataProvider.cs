namespace IngameScript.Pulse.CommandInteface
{
    public interface IDataProvider
    {
        string Get { get; }

        bool IsEmpty { get; }
    }
}

namespace Locafi.Client.Contract.Config
{
    public interface ISerialiserService
    {
        string Serialise(object obj);
        T Deserialise<T>(string json);
    }
}

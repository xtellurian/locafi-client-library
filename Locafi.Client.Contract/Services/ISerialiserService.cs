namespace Locafi.Client.Contract.Services
{
    public interface ISerialiserService
    {
        string Serialise(object obj);
        T Deserialise<T>(string json);
    }
}

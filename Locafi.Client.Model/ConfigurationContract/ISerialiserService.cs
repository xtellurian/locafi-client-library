namespace Locafi.Client.Model.ConfigurationContract
{
    public interface ISerialiserService
    {
        string Serialise(object obj);
        T Deserialise<T>(string json);
    }
}

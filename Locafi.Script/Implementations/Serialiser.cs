using Locafi.Client.Contract.Config;
using Newtonsoft.Json;

namespace Locafi.Script.Implementations
{
    public class Serialiser : ISerialiserService
    {
        public string Serialise(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialise<T>(string json) where T : new()
        {

            var result =  JsonConvert.DeserializeObject<T>(json);

            return result;

        }
    }
}

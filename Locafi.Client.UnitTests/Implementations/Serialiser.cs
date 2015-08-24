using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Services;
using Newtonsoft.Json;

namespace Locafi.Client.UnitTests.Implementations
{
    public class Serialiser : ISerialiserService
    {
        public string Serialise(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialise<T>(string json)
        {

            var result =  JsonConvert.DeserializeObject<T>(json);

            return result;

        }
    }
}

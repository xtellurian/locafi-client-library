﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Newtonsoft.Json;

namespace Locafi.Client.UnitTests.Implementations
{
    public class Serialiser : ISerialiserService
    {
        public string Serialise(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialise<T>(string json) where T : new()
        {
            var deserializerSettings = new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset
            };

            var result =  JsonConvert.DeserializeObject<T>(json, deserializerSettings);

            return result;

        }
    }
}

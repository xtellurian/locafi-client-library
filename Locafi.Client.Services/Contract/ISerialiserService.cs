using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Locafi.Client.Services.Contract
{
    public interface ISerialiserService
    {
        string Serialise(object obj);
        T Deserialise<T>(string json);
    }
}

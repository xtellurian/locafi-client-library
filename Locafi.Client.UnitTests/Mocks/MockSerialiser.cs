using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;

namespace Locafi.Client.UnitTests.Mocks
{
    internal class MockSerialiser : ISerialiserService
    {
        public string Serialise(object obj)
        {
            return "Mock " + obj.GetType().FullName;
        }

        public T Deserialise<T>(string json) where T : new()
        {
            return new T();
        }
    }
}

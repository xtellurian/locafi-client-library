using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Crypto;
using Locafi.Client.Crypto;

namespace Locafi.Client.UnitTests
{
    public static class ServiceContainer
    {
        public static ISha256HashService HashService => new Sha256HashService();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Data
{
    public class AuthorizationTokenDto
    {

        public bool success { get; set; }
        public Tokens tokens { get; set; }
        public IList<string> messages { get; set; }

    }

    public class Tokens
    {
        public string Token { get; set; }
        public string Refresh { get; set; }
    }
}

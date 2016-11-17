using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class AuthenticationUri
    {
        public static string ServiceName => "Authentication";
        public static string Login => "Login";
        public static string RefreshLogin => "RefreshLogin";
        public static string AgentLogin => "AgentLogin";
        public static string RefreshAgentLogin => "RefreshAgentLogin";
        public static string Register => "Register";
    }
}

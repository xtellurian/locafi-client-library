using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Dto.Authentication;

namespace Locafi.Client.UnitTests
{
    public static class StringConstants
    {
       // public static string UserName => "admintester";
        //public static string Password => "t3ster";
        //public static string TestingEmailAddress => "testing@ramp.com.au";
        //public static string ReaderUserName => "037013190748";
        //public static string Secret => "Locafi_";
        public static TokenGroup Tokens { get; set; }
        public static string BaseUrl => ConfigurationManager.AppSettings["baseUrl"] ?? "Not Found";
        public static string Token { get; set; }
        public static string EmailAddress { get; set; }
        public static string Password { get; set; }
    }
}

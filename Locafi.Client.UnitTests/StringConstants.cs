//#define MB_LOCAL
#define PURU_V3
//#define PURU_LOCAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.UnitTests
{
    public static class StringConstants
    {
       // public static string UserName => "admintester";
        public static string PortalUsername => "0123456789";
        public static string Secret => "Locafi_";
#if MB_LOCAL
        public static string BaseUrl => @"http://locafiv2demo.azurewebsites.net/api";
        public static string Password => "ramp123";
        public static string TestingEmailAddress => "admin@ramp.com.au";
#else
#if PURU_V3
        public static string BaseUrl => @"http://navigatorapi.azurewebsites.net/api/";
        public static string Password => "ramp123";
        public static string TestingEmailAddress => "hh@ramp.com.au";
#else
#if PURU_LOCALk
        public static string BaseUrl => @"http://192.168.0.51:52117/api/";
        public static string Password => "ramp123";
        public static string TestingEmailAddress => "hh@ramp.com.au";
#else
        public static string BaseUrl => @"http://legacylocafiapiv3.azurewebsites.net/api/";
        public static string Password => "t3ster";
        public static string TestingEmailAddress => "testing@ramp.com.au";
#endif
#endif
#endif
    }
}

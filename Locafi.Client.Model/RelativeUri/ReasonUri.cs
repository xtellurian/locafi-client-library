using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.Uri
{
    public static class ReasonUri
    {
        public static string ServiceName => "Reasons";
        public static string GetReasons => "GetFilteredReasons";
        public static string CreateReason => "CreateReason";
        public static string UpdateReason => "Update";

        public static string GetReason(Guid id)
        {
            return $"GetReason/{id}";
        }

        public static string DeleteReason(Guid id)
        {
            return $"DeleteReason/{id}";
        }
    }
}

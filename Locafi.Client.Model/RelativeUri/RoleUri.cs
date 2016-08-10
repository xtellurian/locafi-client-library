using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Uri
{
    public static class RoleUri
    {
        public static string ServiceName => "Roles";
        public static string GetRoles => "GetFilteredRoles";
        public static string CreateRole => "CreateRole";
        public static string UpdateRole => "UpdateRole";
        public static string GetLoggedInUserRole => "GetLoggedInUserRole";

        public static string GetRole(Guid id)
        {
            return $"GetRole/{id}";
        }

        public static string DeleteRole(Guid id)
        {
            return $"DeleteRole/{id}";
        }

    }
}

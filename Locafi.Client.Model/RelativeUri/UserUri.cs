using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class UserUri
    {
        public static string ServiceName => "Users";
        public static string CreateUser => "CreateUser";
        public static string GetUser(Guid id)
        {
            return $"GetUser/{id}";
        }

        public static string UpdateUser(Guid userId)
        {
            return $"{GetUser(userId)}/UpdateUser";
        }

        public static string DeleteUser(Guid id)
        {
            return $"DeleteUser/{id}";
        }
    }
}

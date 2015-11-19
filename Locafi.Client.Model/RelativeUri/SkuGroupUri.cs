using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class SkuGroupUri
    {
        public static string ServiceName => "SkuGroup";

        public static string GetSkuGroups => "GetSkuGroups";

        public static string GetSkuGroupDetail(Guid id)
        {
            return $"GetSkuGroup/{id}";
        }
        public static string CreateSkuGroup => "CreateSkuGroup";

        public static string UpdateSkuGroup(Guid id)
        {
            return $"GetSkuGroup/{id}/UpdateSkuGroup";
        }

        public static string DeleteSkuGroup(Guid id)
        {
            return $"DeleteSkuGroup/{id}";
        }

        public static class Names
        {
            public static string GetNames => "GetSkuGroupNames";

            public static string GetSkuGroupNameDetail(Guid id)
            {
                return $"GetSkuGroupName/{id}";
            }

            public static string CreateSkuGroupName => "CreateSkuGroupName";

            public static string UpdateSkuGroupName(Guid id)
            {
                return $"GetSkuGroupName/{id}/UpdateSkuGroupName";
            }

            public static string DeleteSkuGroupName(Guid id)
            {
                return $"DeleteSkuGroupNames/{id}";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class SkuGroupUri
    {
        public static string ServiceName => "SkuGroups";

        public static string GetSkuGroups => "GetFilteredSkuGroups";

        public static string GetSkyGroupsForPlace(Guid placeId)
        {
            return $"GetSkuGroupForPlace/{placeId}";
        }
        public static string GetSkuGroupDetail(Guid id)
        {
            return $"GetSkuGroup/{id}";
        }
        public static string CreateSkuGroup => "CreateSkuGroup";

        public static string UpdateSkuGroup => @"UpdateSkuGroup";

        public static string DeleteSkuGroup(Guid id)
        {
            return $"DeleteSkuGroup/{id}";
        }

        public static class Names
        {
            public static string GetNames => "GetFilteredSkuGroupNames";

            public static string GetSkuGroupNameDetail(Guid id)
            {
                return $"GetSkuGroupName/{id}";
            }

            public static string CreateSkuGroupName => "CreateSkuGroupName";

            public static string UpdateSkuGroupName => @"UpdateSkuGroupName";

            public static string DeleteSkuGroupName(Guid id)
            {
                return $"DeleteSkuGroupName/{id}";
            }
        }
    }
}

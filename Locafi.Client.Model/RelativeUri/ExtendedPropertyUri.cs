using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Uri
{
    public static class ExtendedPropertyUri
    {
        public static string ServiceName => "ExtendedProperties";
        public static string GetExtendedProperties => "GetFilteredExtendedProperties";
        public static string CreateExtendedProperty => "CreateExtendedProperty";
        public static string UpdateExtendedProperty => "UpdateExtendedProperty";

        public static string GetExtendedProperty(Guid id)
        {
            return $"GetExtendedProperty/{id}";
        }

        public static string DeleteExtendedProperty(Guid id)
        {
            return $"DeleteExtendedProperty/{id}";
        }

    }
}

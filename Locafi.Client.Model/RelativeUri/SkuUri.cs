using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.Uri
{
    public static class SkuUri
    {
        public static string ServiceName => "Skus";
        public static string CreateSku => "CreateSku";
        public static string GetSkus => "GetSkus";

        public static string GetSkuDetail(Guid id)
        {
            return $"GetSkuDetail/{id}";
        }

        public static string DeleteSku(Guid id)
        {
            return $"DeleteSku/{id}";
        }
    }
}

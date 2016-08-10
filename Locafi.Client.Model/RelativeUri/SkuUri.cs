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
        public static string GetSkus => "GetFilteredSkus";
        public static string SearchSkus => "CustomSkuSearch";
        public static string UpdateSku => "UpdateSku";

        public static string GetSkuDetail(Guid id)
        {
            return $"GetSku/{id}";
        }

        public static string DeleteSku(Guid id)
        {
            return $"DeleteSku/{id}";
        }
    }
}

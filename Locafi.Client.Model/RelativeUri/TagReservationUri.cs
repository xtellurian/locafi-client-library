using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class TagReservationUri
    {
        public static string SerivceName => "TagReservations";

        public static string Reserve(Guid skuId, int quantity)
        {
            return $"{skuId}/Reserve/{quantity}";
        }
    }
}

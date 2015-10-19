using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locafi.Client.Model.RelativeUri
{
    public static class TagReservationUri
    {
        public static string ServiceName => "TagReservations";

        public static string ReserveBySku(Guid skuId, int quantity)
        {
            return $"{skuId}/ReserveBySku/{quantity}";
        }

        public static string ReserveByGtin(string gtin, int quantity)
        {
            return $"{gtin}/ReserveByGtin/{quantity}";
        }
    }
}

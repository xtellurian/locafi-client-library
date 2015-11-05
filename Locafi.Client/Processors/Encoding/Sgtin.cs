using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Model.RFID;

namespace Locafi.Client.Processors.Encoding
{
    public static class Sgtin
    {
        public static bool IsTagOfSkuType(IRfidTag tag, string gtin)
        {
            var tagGtin = SgtinTagCoder.GetGtin(tag.TagNumber);
            return string.Equals(tagGtin, gtin);
        }

        public static bool IsSgtinTag(IRfidTag tag)
        {
            return SgtinTagCoder.IsSgtinFormat(tag.TagNumber);
        }

        public static string GetGtin(IRfidTag tag)
        {
            return SgtinTagCoder.GetGtin(tag.TagNumber);
        }
    }
}

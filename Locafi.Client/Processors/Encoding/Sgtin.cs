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
        public static bool HasSgtin(this IRfidTag tag, string gtin = null)
        {
            if(gtin==null) return SgtinTagCoder.IsSgtinFormat(tag.TagNumber); // if there is no gtin to check against

            var tagGtin = SgtinTagCoder.GetGtin(tag.TagNumber);
            return string.Equals(tagGtin, gtin);
        }

        public static string GetGtin(this IRfidTag tag)
        {
            return SgtinTagCoder.GetGtin(tag.TagNumber);
        }
    }
}

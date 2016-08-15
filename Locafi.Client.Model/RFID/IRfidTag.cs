using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.RFID
{
    public interface IRfidTag
    {
        string TagNumber { get; }
        int ReadCount { get; set; }  // number of times the tag was read during this inventory/allocation/receive etc
        double Rssi { get; set; } // average RSSI of the tag during this inventory/allocation/receive et
    }
}

using System;
using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.RFID
{
    public interface IRfidTag
    {
        string TagNumber { get; }
        //TagType TagType { get; set; }
        int ReadCount { get; set; }  // number of times the tag was read during this inventory/allocation/receive etc
        double AverageRssi { get; set; } // average RSSI of the tag during this inventory/allocation/receive etc
        //DateTime Timestamp { get; set; } //time of first read
    }
}

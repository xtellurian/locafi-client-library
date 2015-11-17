using Locafi.Client.Model.Enums;

namespace Locafi.Client.Model.RFID
{
    public interface IRfidTag
    {
        string TagNumber { get; set; }
//        TagType TagType { get; set; } // - removed as rarely used, and simplifies
    }
}

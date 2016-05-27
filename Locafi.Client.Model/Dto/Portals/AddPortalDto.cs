namespace Locafi.Client.Model.Dto.Portals
{
    public class AddPortalDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public int MaxRfidReaders { get; set; }
        public int MaxPeripheralDevices { get; set; }
    }
}

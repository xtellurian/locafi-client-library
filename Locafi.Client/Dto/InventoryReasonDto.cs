namespace Locafi.Client.Data
{
    public class InventoryReasonDto
    {
        public string ItemId { get; set; }  // the item that the reason relates to
        public string ReasonId { get; set; } // the reason for the item
    }
}

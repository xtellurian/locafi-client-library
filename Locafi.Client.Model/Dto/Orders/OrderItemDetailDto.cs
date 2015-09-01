namespace Locafi.Client.Model.Dto.Orders
{
    public class OrderItemDetailDto
    {
        public string TagNumber { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public bool IsAllocated { get; set; }
        public bool IsReceived { get; set; }
    }
}
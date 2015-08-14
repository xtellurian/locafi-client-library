namespace Locafi.Entity.Dto
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
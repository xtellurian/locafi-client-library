namespace Locafi.Client.Data
{
    public class OrderSkuDetailDto
    {
        public string TypeId { get; set; }
        public int Quantity { get; set; }
        public int PackingSize { get; set; }
        public int QtyAllocated { get; set; }
        public int QtyReceived { get; set; }
        public string SgtinRef { get; set; }
        public string Name { get; set; }
    }
}
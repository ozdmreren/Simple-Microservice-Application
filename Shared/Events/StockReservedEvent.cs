namespace Shared.Events
{
    public class StockReservedEvent
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public float TotalPrice { get; set; }
    }
}
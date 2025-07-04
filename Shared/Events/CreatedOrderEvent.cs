namespace Shared.Events
{
    public class CreatedOrderEvent
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public List<CreatedOrderItem> OrderItems { get; set; }
        public float TotalPrice { get; set; }
    }
}
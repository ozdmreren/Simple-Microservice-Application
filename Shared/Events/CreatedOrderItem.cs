namespace Shared.Events
{
    public class CreatedOrderItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
    }
}
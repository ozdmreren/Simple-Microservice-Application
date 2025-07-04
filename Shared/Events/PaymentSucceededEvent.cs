namespace Shared.Events
{
    public class PaymentSucceededEvent
    {
        public PaymentSucceededEvent(int customerId, int orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }
        public int CustomerId { get; init; }
        public int OrderId { get; init; }

    }
}
namespace Shared.Events
{
    public class PaymentFailedEvent
    {
        private readonly string Message;

        public PaymentFailedEvent(string message) => Message = message;
    }
}
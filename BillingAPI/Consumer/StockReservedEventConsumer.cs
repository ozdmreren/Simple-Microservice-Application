using System.Text.Json;
using MassTransit;
using Shared.Events;

namespace BillingAPI.Consumer
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            //... payment process

            bool isPaymentSuccessfull = true;

            if (isPaymentSuccessfull)
            {
                PaymentSucceededEvent proc = new  PaymentSucceededEvent(
                    customerId:context.Message.CustomerId,
                    orderId:context.Message.OrderId
                );

                await _publishEndpoint.Publish(proc);

                return;
            }
            else
            {
                await _publishEndpoint.Publish(new PaymentFailedEvent(
                    $"CustomerId: {context.Message.CustomerId}\nOrderId: {context.Message.OrderId}\nTotalPrice: {context.Message.TotalPrice}\nSipariş Ödenemedi"));

                return;
            }
        }
    }
}
using System.Text.Json;
using MassTransit;
using Shared.Events;
using StockAPI.Data;
using StockAPI.Models;

namespace StockAPI.Consumer
{
    public class CreatedOrderEventConsumer : IConsumer<CreatedOrderEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly StockDbContext _stockDbContext;
        public CreatedOrderEventConsumer(StockDbContext stockDbContext, IPublishEndpoint publishEndpoint)
        {
            _stockDbContext = stockDbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<CreatedOrderEvent> context)
        {
            foreach (var it in context.Message.OrderItems)
            {
                var stock = _stockDbContext.Products.Find(it.ProductId);

                Console.WriteLine(stock);

                if (stock is null || stock.Quantity - it.Quantity < 0)
                {
                    throw new Exception("Yetersiz Ürün");
                }
                else
                {
                    stock.Quantity = stock.Quantity - it.Quantity;
                }

                _stockDbContext.SaveChanges();
            }

            float totalPrice = 0f;

            foreach (var item in context.Message.OrderItems)
            {
                var stock = _stockDbContext.Products.Find(item.ProductId);
                totalPrice = totalPrice + (stock.Quantity * item.UnitPrice);
            }

            StockReservedEvent reserved = new StockReservedEvent()
            {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
                TotalPrice = totalPrice
            };

            await _publishEndpoint.Publish(reserved);


            return;
        }
    }
}
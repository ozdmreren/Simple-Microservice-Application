
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Models;
using Shared.Dtos;
using Shared.Events;

namespace OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly OrderDbContext _orderDbContext;
        public OrderService(OrderDbContext orderDbContext, IPublishEndpoint publishEndpoint)
        {
            _orderDbContext = orderDbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Order> CreateOrder(OrderDto order)
        {


            Order createdOrder = new Order()
            {
                CustomerId = order.CustomerId,
                OrderItems = order.OrderItems.Select(it => new OrderItem()
                {
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    TotalPrice = it.Quantity * it.UnitPrice,
                }).ToList()
            };

            await _orderDbContext.Orders.AddAsync(createdOrder);
            await _orderDbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(new CreatedOrderEvent()
            {
                CustomerId = createdOrder.CustomerId,
                OrderId = createdOrder.Id,
                OrderItems = createdOrder.OrderItems.Select(it => new CreatedOrderItem()
                {
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    UnitPrice = it.UnitPrice
                }).ToList(),
                TotalPrice = order.OrderItems.Sum(x => x.Quantity * x.UnitPrice)
            });

            return createdOrder;
        }
        public async Task<List<Order>> GetOrder(int customerId)
        {
            List<Order> orders = await _orderDbContext.Orders
            .Where(q => q.CustomerId == customerId)
            .Include(q=>q.OrderItems)
            .ToListAsync();

            return orders;
        }
    }
}
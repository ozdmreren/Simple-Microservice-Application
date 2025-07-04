using OrderAPI.Models;
using Shared.Dtos;

namespace OrderAPI.Services
{
    public interface IOrderService
    {
        public Task<Order> CreateOrder(OrderDto orderDto);
        public Task<List<Order>> GetOrder(int customerId);
    }
}
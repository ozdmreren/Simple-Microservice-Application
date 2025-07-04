using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Services;
using Serilog;
using Shared.Dtos;

namespace OrderAPI.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("Store")]
        public async Task<Order> Store([FromBody] OrderDto order)
        {
            var ord = await _orderService.CreateOrder(order);

            Log.Information($"-----Order created: {JsonSerializer.Serialize(ord)}");

            return ord;
        }
        [HttpGet("GetOrder/{customerId}")]
        public async Task<List<Order>> GetOrder(int customerId)
        {
            List<Order> orders = await _orderService.GetOrder(customerId);

            return orders;
        }
    }    
}
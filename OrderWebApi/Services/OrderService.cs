using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWebApi.Services;
using OrderWebApi.Context;
using OrderWebApi.Model;

namespace OrderWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _dbContext;

        public OrderService(OrderDbContext orderDbContext)
        {
            _dbContext = orderDbContext;
        }
        public async Task Create(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _dbContext.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orderList = from p in _dbContext.Orders
                              orderby p.OrderedOn
                              select p;
            return await orderList.ToListAsync();
        }

        public async Task<Order?> GetOrderById(Guid orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            return order;
        }

        public async Task Update(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}

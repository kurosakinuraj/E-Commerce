using Microsoft.AspNetCore.Mvc;
using OrderWebApi.Model;

namespace OrderWebApi.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrders();
        Task<Order?> GetOrderById(Guid orderId);
        Task Create(Order order);
        Task Update(Order order);
        Task Delete(Guid orderId);
    }
}

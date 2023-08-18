using Microsoft.AspNetCore.Mvc;
using ProductWebApi.Model;

namespace ProductWebApi.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product?> GetProductById(Guid productId);
        Task Create(Product product);
        Task Update(Product product);
        Task Delete(Guid productId);
    }
}

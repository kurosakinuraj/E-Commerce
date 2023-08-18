using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Context;
using ProductWebApi.Model;

namespace ProductWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _dbContext;

        public ProductService(ProductDbContext productDbContext)
        {
            _dbContext = productDbContext;
        }
        public async Task Create(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var productList = from p in _dbContext.Products
                              orderby p.ProductPrice
                              select p;
            return await productList.ToListAsync();
        }

        public async Task<Product?> GetProductById(Guid productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            return product;
        }

        public async Task Update(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}

using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Context;
using ProductWebApi.Model;
using ProductWebApi.Services;

namespace ProductApiUnitTest.System.Services
{
    public class TestProductService : IDisposable
    {
        private readonly ProductDbContext _dbContext;
        public TestProductService()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _dbContext = new ProductDbContext(options);
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnListOfProduct()
        {
            //Arrage
            _dbContext.AddRange(GenerateData(10));
            _dbContext.SaveChanges();

            var sut = new ProductService(_dbContext);

            //Act
            var result = await sut.GetAllProducts();

            //Assert
            result.Count().Should().Be(10);
        }
        private List<Product> GenerateData(int count)
        {
            var fakeProduct = new Faker<Product>()
            .RuleFor(p => p.ProductId, f => Guid.NewGuid())
            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
            .RuleFor(p => p.ProductCode, f => f.Commerce.Ean13())
            .RuleFor(p => p.ProductDescription, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.ProductPrice, f => f.Random.Decimal(1, 1000));

            return fakeProduct.Generate(count);

        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}

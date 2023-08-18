using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderWebApi.Context;
using OrderWebApi.Model;
using OrderWebApi.Services;

namespace OrderApiUnitTest.System.Services
{
    public class TestOrderService : IDisposable
    {
        private readonly OrderDbContext _dbContext;
        public TestOrderService()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _dbContext = new OrderDbContext(options);
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnListOfOrder()
        {
            //Arrage
            _dbContext.AddRange(GenerateData(10));
            _dbContext.SaveChanges();

            var sut = new OrderService(_dbContext);

            //Act
            var result = await sut.GetAllOrders();

            //Assert
            result.Count().Should().Be(10);
        }
        private List<Order> GenerateData(int count)
        {
            var fakeOrder = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => Guid.NewGuid())
            .RuleFor(o => o.OrderedOn, f => f.Date.Past())
            .RuleFor(o => o.ProductId, f => Guid.NewGuid())
            .RuleFor(o => o.UnitPrice, f => f.Finance.Amount(10, 1000))
            .RuleFor(o => o.Quantity, f => f.Random.Decimal(1, 100));

            return fakeOrder.Generate(count);

        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}

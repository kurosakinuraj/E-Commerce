using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OrderWebApi.Controllers;
using OrderWebApi.Model;
using OrderWebApi.Services;

namespace OrderApiUnitTest.System.Controller
{
    public class TestOrderController
    {
        List<Order> orderList;
        List<Order> emptOrderList;
        public TestOrderController()
        {
            orderList = GenerateData(10);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnListOfOrder()
        {
            //arange
            var orderService = new Mock<IOrderService>();
            orderService.Setup(o => o.GetAllOrders()).ReturnsAsync(orderList);
            var sut = new OrderController(orderService.Object);

            //Act
            var result = await sut.GetAllOrders();

            //Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnOrderById()
        {
            //arange
            var mockOrder = GenerateData(1).FirstOrDefault();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(o => o.GetOrderById(mockOrder.OrderId)).ReturnsAsync(mockOrder);
            var sut = new OrderController(orderService.Object);

            //Act
            var result = await sut.GetOrderById(mockOrder.OrderId);

            //Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Post_OnSuccess_CreateOrder()
        {
            //arange
            var mockOrder = GenerateData(1).FirstOrDefault();
            mockOrder.OrderId = new Guid();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(o => o.Create(mockOrder));
            var sut = new OrderController(orderService.Object);

            //Act
            var result = await sut.Create(mockOrder);

            //Assert
            result.GetType().Should().Be(typeof(OkResult));
            (result as OkResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_OnSuccess_DeleteOrder()
        {
            //arange
            var mockOrder = GenerateData(1).FirstOrDefault();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(o => o.Delete(mockOrder.OrderId));
            var sut = new OrderController(orderService.Object);

            //Act
            var result = await sut.Delete(mockOrder.OrderId);

            //Assert
            result.GetType().Should().Be(typeof(OkResult));
            (result as OkResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_OnSuccess_UpdateOrder()
        {
            //arange
            var mockOrder = GenerateData(1).FirstOrDefault();
            var orderService = new Mock<IOrderService>();
            orderService.Setup(o => o.Update(mockOrder));
            var sut = new OrderController(orderService.Object);

            //Act
            var result = await sut.Update(mockOrder);

            //Assert
            result.GetType().Should().Be(typeof(OkResult));
            (result as OkResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Create_ShouldCallOrderServiceOnce()
        {
            //arange
            var mockOrder = GenerateData(1).FirstOrDefault();
            var orderService = new Mock<IOrderService>();
            var sut = new OrderController(orderService.Object);

            //Act
            var result = await sut.Create(mockOrder);

            //Assert
            orderService.Verify(o => o.Create(mockOrder), Times.Exactly(1));
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
    }
}
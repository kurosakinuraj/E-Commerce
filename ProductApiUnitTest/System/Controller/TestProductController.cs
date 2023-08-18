using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductWebApi.Controllers;
using ProductWebApi.Model;
using ProductWebApi.Services;

namespace ProductApiUnitTest.System.Controller
{
    public class TestProductController
    {
        List<Product> productList;
        List<Product> emptProductList;
        public TestProductController()
        {
            productList = GenerateData(10);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnListOfProduct()
        {
            //arange
            var productService = new Mock<IProductService>();
            productService.Setup(o => o.GetAllProducts()).ReturnsAsync(productList);
            var sut = new ProductController(productService.Object);

            //Act
            var result = await sut.GetAllProducts();

            //Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnProductById()
        {
            //arange
            var mockProduct = GenerateData(1).FirstOrDefault();
            var productService = new Mock<IProductService>();
            productService.Setup(o => o.GetProductById(mockProduct.ProductId)).ReturnsAsync(mockProduct);
            var sut = new ProductController(productService.Object);

            //Act
            var result = await sut.GetProductById(mockProduct.ProductId);

            //Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Post_OnSuccess_CreateProduct()
        {
            //arange
            var mockProduct = GenerateData(1).FirstOrDefault();
            mockProduct.ProductId = new Guid();
            var productService = new Mock<IProductService>();
            productService.Setup(o => o.Create(mockProduct));
            var sut = new ProductController(productService.Object);

            //Act
            var result = await sut.Create(mockProduct);

            //Assert
            result.GetType().Should().Be(typeof(OkResult));
            (result as OkResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_OnSuccess_DeleteProduct()
        {
            //arange
            var mockProduct = GenerateData(1).FirstOrDefault();
            var productService = new Mock<IProductService>();
            productService.Setup(o => o.Delete(mockProduct.ProductId));
            var sut = new ProductController(productService.Object);

            //Act
            var result = await sut.Delete(mockProduct.ProductId);

            //Assert
            result.GetType().Should().Be(typeof(OkResult));
            (result as OkResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Delete_OnSuccess_UpdateProduct()
        {
            //arange
            var mockProduct = GenerateData(1).FirstOrDefault();
            var productService = new Mock<IProductService>();
            productService.Setup(o => o.Update(mockProduct));
            var sut = new ProductController(productService.Object);

            //Act
            var result = await sut.Update(mockProduct);

            //Assert
            result.GetType().Should().Be(typeof(OkResult));
            (result as OkResult).StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Create_ShouldCallProdutServiceOnce()
        {
            //arange
            var mockProduct = GenerateData(1).FirstOrDefault();
            var productService = new Mock<IProductService>();
            var sut = new ProductController(productService.Object);

            //Act
            var result = await sut.Create(mockProduct);

            //Assert
            productService.Verify(o => o.Create(mockProduct), Times.Exactly(1));
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
    }
}
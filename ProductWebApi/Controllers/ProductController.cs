using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductWebApi.Context;
using ProductWebApi.Model;
using ProductWebApi.Services;

namespace ProductWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var productList = await _productService.GetAllProducts();
                if (productList.Count == 0) return NoContent();
                return Ok(Task.FromResult(productList.ToList()));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{productId:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                var product = await _productService.GetProductById(productId);
                if (product == null)
                {
                    return NotFound(product);
                }
                else
                {
                    return Ok(product);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    await _productService.Create(product);
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    await _productService.Update(product);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{productId:Guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            try
            {
                await _productService.Delete(productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}

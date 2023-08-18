using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderWebApi.Context;
using OrderWebApi.Model;
using OrderWebApi.Services;

namespace OrderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orderList = await _orderService.GetAllOrders();
                if (orderList.Count == 0) return NoContent();
                return Ok(Task.FromResult(orderList.ToList()));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{orderId:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            try
            {
                var order = await _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    return NotFound(order);
                }
                else
                {
                    return Ok(order);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest();
                }
                else
                {
                    await _orderService.Create(order);
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
        public async Task<IActionResult> Update(Order order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest();
                }
                else
                {
                    await _orderService.Update(order);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{orderId:Guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            try
            {
                await _orderService.Delete(orderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}

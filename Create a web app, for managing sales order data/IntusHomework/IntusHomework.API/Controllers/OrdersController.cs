using Microsoft.AspNetCore.Mvc;
using IntusHomework.DAL.Interfaces;
using IntusHomework.DTOs;

namespace IntusHomework.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet]
        public async Task<IEnumerable<OrderDTO>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO order)
        {
            await _orderService.CreateAsync(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDTO order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            try
            {
                await _orderService.UpdateAsync(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{orderId}/Windows")]
        public async Task<IActionResult> RemoveWindowsFromOrder(int orderId, [FromBody] List<WindowDTO> windows)
        {
            try
            {
                await _orderService.RemoveWindowsAsync(orderId, windows);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

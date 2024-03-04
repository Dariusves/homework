using Microsoft.AspNetCore.Mvc;
using IntusHomework.DAL.Entities;
using IntusHomework.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using IntusHomework.DTOs;

namespace IntusHomework.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindowsController(IWindowService windowService) : ControllerBase
    {
        private readonly IWindowService _windowService = windowService;

        [HttpGet("{id}")]
        public async Task<ActionResult<WindowDTO>> GetWindow(int id)
        {
            var window = await _windowService.GetWindowByIdAsync(id);
            if (window == null)
            {
                return NotFound();
            }
            return window;
        }

        [HttpPost]
        public async Task<ActionResult<WindowDTO>> PostWindow(WindowDTO window)
        {
            await _windowService.CreateWindowAsync(window);

            return CreatedAtAction(nameof(GetWindow), new { id = window.WindowId }, window);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWindow(int id, WindowDTO window)
        {
            if (id != window.WindowId)
            {
                return BadRequest();
            }

            try
            {
                await _windowService.UpdateWindowAsync(window);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWindow(int id)
        {
            try
            {
                await _windowService.DeleteWindowAsync(id);
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
    }
}

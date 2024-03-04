using IntusHomework.DAL.Entities;
using IntusHomework.DAL.Interfaces;
using IntusHomework.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SubElementsController(ISubElementService subElementService) : ControllerBase
{
    private readonly ISubElementService _subElementService = subElementService;

    [HttpGet("{id}")]
    public async Task<ActionResult<SubElementDTO>> GetSubElement(int id)
    {
        var subElement = await _subElementService.GetSubElementByIdAsync(id);
        if (subElement == null)
        {
            return NotFound();
        }
        return subElement;
    }

    [HttpPost]
    public async Task<ActionResult<SubElementDTO>> PostSubElement(SubElementDTO subElement)
    {
        var createdSubElement = await _subElementService.CreateSubElementAsync(subElement);
        return CreatedAtAction(nameof(GetSubElement), new { id = createdSubElement.SubElementId }, createdSubElement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubElement(int id, SubElementDTO subElement)
    {
        if (id != subElement.SubElementId)
        {
            return BadRequest();
        }
        await _subElementService.UpdateSubElementAsync(subElement);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubElement(int id)
    {
        await _subElementService.DeleteSubElementAsync(id);
        return NoContent();
    }

    [HttpGet("Window/{windowId}")]
    public async Task<ActionResult<IEnumerable<SubElement>>> GetSubElementsByWindowId(int windowId)
    {
        var subElements = await _subElementService.GetSubElementsByWindowIdAsync(windowId);
        if (subElements == null)
        {
            return NotFound();
        }
        return Ok(subElements);
    }
}

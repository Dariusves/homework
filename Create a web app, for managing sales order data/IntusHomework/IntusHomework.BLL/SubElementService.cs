using IntusHomework.DAL.Entities;
using IntusHomework.DAL.Interfaces;
using IntusHomework.DTOs;
using Microsoft.EntityFrameworkCore;

public class SubElementService(ProjectDbContext context) : ISubElementService
{
    private readonly ProjectDbContext _context = context;

    public async Task<SubElementDTO> GetSubElementByIdAsync(int subElementId)
    {
        var subElement = await _context.SubElements.FindAsync(subElementId);
        return subElement != null ? MapToDTO(subElement) : null;
    }

    public async Task<SubElementDTO> CreateSubElementAsync(SubElementDTO subElementDto)
    {
        var subElement = MapToEntity(subElementDto);
        _context.SubElements.Add(subElement);
        await _context.SaveChangesAsync();
        return MapToDTO(subElement);
    }

    public async Task UpdateSubElementAsync(SubElementDTO subElementDto)
    {
        var subElement = await _context.SubElements.FindAsync(subElementDto.SubElementId);
        if (subElement != null)
        {
            // Map DTO properties back to the entity
            subElement.Element = subElementDto.Element;
            subElement.Type = subElementDto.Type;
            subElement.Width = subElementDto.Width;
            subElement.Height = subElementDto.Height;
            subElement.WindowId = subElementDto.WindowId;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteSubElementAsync(int subElementId)
    {
        var subElement = await _context.SubElements.FindAsync(subElementId);
        if (subElement != null)
        {
            _context.SubElements.Remove(subElement);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<SubElementDTO>> GetSubElementsByWindowIdAsync(int windowId)
    {
        var subElements = await _context.SubElements
                             .Where(se => se.WindowId == windowId)
                             .ToListAsync();
        return subElements.Select(se => MapToDTO(se));
    }

    // Helper methods for DTO to Entity mapping
    private SubElementDTO MapToDTO(SubElement subElement)
    {
        return new SubElementDTO
        {
            SubElementId = subElement.SubElementId,
            Element = subElement.Element,
            Type = subElement.Type,
            Width = subElement.Width,
            Height = subElement.Height,
            WindowId = subElement.WindowId,
        };
    }

    private SubElement MapToEntity(SubElementDTO subElementDto)
    {
        return new SubElement
        {
            SubElementId = subElementDto.SubElementId,
            Element = subElementDto.Element,
            Type = subElementDto.Type,
            Width = subElementDto.Width,
            Height = subElementDto.Height,
            WindowId = subElementDto.WindowId
        };
    }
}

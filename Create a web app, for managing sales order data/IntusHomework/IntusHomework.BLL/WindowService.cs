using IntusHomework.DAL.Entities;
using IntusHomework.DAL.Interfaces;
using IntusHomework.DTOs;
using Microsoft.EntityFrameworkCore;

public class WindowService(ProjectDbContext context) : IWindowService
{
    private readonly ProjectDbContext _context = context;

    public async Task<WindowDTO> GetWindowByIdAsync(int windowId)
    {
        var window = await _context.Windows
            .Include(w => w.SubElements)
            .SingleOrDefaultAsync(w => w.WindowId == windowId);

        return window != null ? MapToDTO(window) : null;
    }

    public async Task<WindowDTO> CreateWindowAsync(WindowDTO windowDto)
    {
        var window = MapToEntity(windowDto);
        _context.Windows.Add(window);
        await _context.SaveChangesAsync();

        return MapToDTO(window);
    }

    public async Task UpdateWindowAsync(WindowDTO windowDto)
    {
        var window = await _context.Windows
            .Include(w => w.SubElements)
            .SingleOrDefaultAsync(w => w.WindowId == windowDto.WindowId);

        if (window != null)
        {
            window.Name = windowDto.Name;
            window.QuantityOfWindows = windowDto.QuantityOfWindows;
            window.TotalSubElements = windowDto.TotalSubElements;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteWindowAsync(int windowId)
    {
        var window = await _context.Windows.FindAsync(windowId);
        if (window != null)
        {
            _context.Windows.Remove(window);
            await _context.SaveChangesAsync();
        }
    }

    private WindowDTO MapToDTO(Window window)
    {
        return new WindowDTO
        {
            WindowId = window.WindowId,
            Name = window.Name,
            QuantityOfWindows = window.QuantityOfWindows,
            TotalSubElements = window.TotalSubElements,
            SubElements = window.SubElements != null
                          ? window.SubElements.Select(MapToDTO).ToList()
                          : new List<SubElementDTO>(),
            OrderId = window.OrderId
        };
    }

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

    private Window MapToEntity(WindowDTO windowDto)
    {
        return new Window
        {
            WindowId = windowDto.WindowId,
            Name = windowDto.Name,
            QuantityOfWindows = windowDto.QuantityOfWindows,
            TotalSubElements = windowDto.TotalSubElements,
            SubElements = windowDto.SubElements != null
                          ? windowDto.SubElements.Select(MapToEntity).ToList()
                          : new List<SubElement>(),
            OrderId = windowDto.OrderId,
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

using IntusHomework.DTOs;

namespace IntusHomework.DAL.Interfaces
{
    public interface IWindowService
    {
        Task<WindowDTO> GetWindowByIdAsync(int windowId);
        Task<WindowDTO> CreateWindowAsync(WindowDTO window);
        Task UpdateWindowAsync(WindowDTO window);
        Task DeleteWindowAsync(int windowId);
    }
}

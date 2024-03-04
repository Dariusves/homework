using IntusHomework.DTOs;

namespace IntusHomework.DAL.Interfaces
{
    public interface ISubElementService
    {
        Task<SubElementDTO> GetSubElementByIdAsync(int subElementId);
        Task<SubElementDTO> CreateSubElementAsync(SubElementDTO subElement);
        Task UpdateSubElementAsync(SubElementDTO subElement);
        Task DeleteSubElementAsync(int subElementId);
        Task<IEnumerable<SubElementDTO>> GetSubElementsByWindowIdAsync(int windowId);
    }
}

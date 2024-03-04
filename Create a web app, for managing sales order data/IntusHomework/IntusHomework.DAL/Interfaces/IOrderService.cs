using IntusHomework.DAL.Entities;
using IntusHomework.DTOs;

namespace IntusHomework.DAL.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderAsync(int orderId);
        Task CreateAsync(OrderDTO order);
        Task UpdateAsync(OrderDTO order);
        Task DeleteAsync(int orderId);
        Task RemoveWindowsAsync(int orderId, IEnumerable<WindowDTO> windows);
    }
}

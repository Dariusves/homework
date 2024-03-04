using IntusHomework.DAL.Entities;
using IntusHomework.DAL.Interfaces;
using IntusHomework.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IntusHomework.BLL
{
    public class OrderService(ProjectDbContext context) : IOrderService
    {
        private readonly ProjectDbContext _context = context;

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var ordersQuery = _context.Orders
                .Include(o => o.Windows)
                    .ThenInclude(w => w.SubElements);

            var orderDTOs = await ordersQuery
                .Select(order => new OrderDTO
                {
                    OrderId = order.OrderId,
                    Name = order.Name,
                    State = order.State,
                    Windows = order.Windows.Select(w => new WindowDTO
                    {
                        WindowId = w.WindowId,
                        Name = w.Name,
                        QuantityOfWindows = w.QuantityOfWindows,
                        TotalSubElements = w.TotalSubElements,
                        OrderId = w.OrderId,
                        SubElements = w.SubElements.Select(se => new SubElementDTO
                        {
                            SubElementId = se.SubElementId,
                            Element = se.Element,
                            Type = se.Type,
                            Width = se.Width,
                            Height = se.Height,
                            WindowId = se.WindowId,
                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            return orderDTOs;
        }

        public async Task<OrderDTO> GetOrderAsync(int orderId)
        {
            var order = await _context.Orders
                          .Include(o => o.Windows)
                          .ThenInclude(w => w.SubElements)
                          .SingleOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return new();
            }

            var orderDTO = new OrderDTO
            {
                OrderId = order.OrderId,
                Name = order.Name,
                State = order.State,
                Windows = order.Windows.Select(w => new WindowDTO
                {
                    WindowId = w.WindowId,
                    Name = w.Name,
                    QuantityOfWindows = w.QuantityOfWindows,
                    TotalSubElements = w.TotalSubElements,
                    OrderId = w.OrderId,
                    SubElements = w.SubElements.Select(se => new SubElementDTO
                    {
                        SubElementId = se.SubElementId,
                        Element = se.Element,
                        Type = se.Type,
                        Width = se.Width,
                        Height = se.Height,
                        WindowId = se.WindowId,
                    }).ToList()
                }).ToList()
            };

            return orderDTO;
        }

        public async Task CreateAsync(OrderDTO orderDto)
        {
            var order = new Order
            {
                Name = orderDto.Name,
                State = orderDto.State,
                Windows = orderDto.Windows?.Select(w => new Window
                {
                    Name = w.Name,
                    QuantityOfWindows = w.QuantityOfWindows,
                    TotalSubElements = w.TotalSubElements,
                    SubElements = w.SubElements?.Select(se => new SubElement
                    {
                        Element = se.Element,
                        Type = se.Type,
                        Width = se.Width,
                        Height = se.Height
                    }).ToList() ?? new List<SubElement>()
                }).ToList() ?? new List<Window>()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderDTO orderDto)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Windows)
                    .ThenInclude(w => w.SubElements)
                .SingleOrDefaultAsync(o => o.OrderId == orderDto.OrderId);

            if (existingOrder != null)
            {
                existingOrder.Name = orderDto.Name;
                existingOrder.State = orderDto.State;
                foreach (var windowDto in orderDto.Windows)
                {
                    var window = existingOrder.Windows
                        .SingleOrDefault(w => w.WindowId == windowDto.WindowId);

                    if (window != null)
                    {
                        window.Name = windowDto.Name;
                        window.QuantityOfWindows = windowDto.QuantityOfWindows;
                        window.TotalSubElements = windowDto.TotalSubElements;
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Order not found.");
            }
        }


        public async Task DeleteAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Order not found.");
            }
        }

        public async Task RemoveWindowsAsync(int orderId, IEnumerable<WindowDTO> windows)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == orderId);
            if (!orderExists)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            foreach (var window in windows)
            {
                var windowsToDelete = _context.Windows.Where(o => o.WindowId == window.WindowId);
            }

            await _context.SaveChangesAsync();
        }
    }
}

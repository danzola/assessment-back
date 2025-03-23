using CoE.Assessment.Orders.Data.Contexts;
using CoE.Assessment.Orders.Domain.Models;
using CoE.Assessment.Orders.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoE.Assessment.Orders.Data.Repositories
{
    public class OrdersRepository(OrdersDbContext context) : IOrdersRepository
    {
        private readonly OrdersDbContext _context = context;        
        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }
        public async Task<IEnumerable<Order>> GetAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> DeleteAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return null;
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}

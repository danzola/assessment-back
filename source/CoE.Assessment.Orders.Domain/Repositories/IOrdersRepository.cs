using CoE.Assessment.Orders.Domain.Models;

namespace CoE.Assessment.Orders.Domain.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAsync();
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<Order?> DeleteAsync(int orderId);
    }
}

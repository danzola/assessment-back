using CoE.Assessment.Orders.Application.Models;

namespace CoE.Assessment.Orders.Application.Interfaces
{
    public interface IOrderService
    {
        Task Create(NewOrder newOrderDto);
        Task<GetOrder> GetByIdAsync(int id);
        Task DeleteByIdAsync(int id);
        Task<GetOrder> Update(int id, UpdateOrder updateOrder);
    }
}

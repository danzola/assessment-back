using CoE.Assessment.Orders.Application.Models;

namespace CoE.Assessment.Orders.Application.Interfaces
{
    public interface IOrderService
    {
        Task Create(NewOrderDto newOrderDto);
    }
}

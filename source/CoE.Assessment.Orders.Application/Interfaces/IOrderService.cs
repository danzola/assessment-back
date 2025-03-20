using CoE.Assessment.Orders.Application.Models;

namespace CoE.Assessment.Orders.Application.Interfaces
{
    interface IOrderService
    {
        Task Create(NewOrderDto newOrderDto);
    }
}

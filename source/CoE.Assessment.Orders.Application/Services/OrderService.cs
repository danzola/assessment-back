using CoE.Assessment.Domain.Bus;
using CoE.Assessment.Orders.Application.Interfaces;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Domain.Commands;

namespace CoE.Assessment.Orders.Application.Services
{
    public class OrderService(IEventBus eventBus) : IOrderService
    {
        private readonly IEventBus _eventBus = eventBus;
        public async Task Create(NewOrderDto newOrderDto)
        {
            var verifyProductCommand = new VerifyProductCommand(newOrderDto.CustomerId, newOrderDto.ProductId, newOrderDto.Quantity);
            await _eventBus.SendCommand(verifyProductCommand);
        }
    }
}

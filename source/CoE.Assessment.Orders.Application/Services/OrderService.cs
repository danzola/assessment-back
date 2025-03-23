using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Orders.Application.Interfaces;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Domain.Commands;

namespace CoE.Assessment.Orders.Application.Services
{
    public class OrderService(ICommandBus commandBus) : IOrderService
    {
        private readonly ICommandBus _commandBus = commandBus;
        public async Task Create(NewOrderDto newOrderDto)
        {
            var verifyProductCommand = new VerifyProductCommand(newOrderDto.CustomerId, newOrderDto.ProductId, newOrderDto.Quantity);
            await _commandBus.SendCommand(verifyProductCommand);
        }
    }
}

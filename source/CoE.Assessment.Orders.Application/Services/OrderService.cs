using AutoMapper;
using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Orders.Application.Interfaces;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Domain.Commands;
using CoE.Assessment.Orders.Domain.Repositories;

namespace CoE.Assessment.Orders.Application.Services
{
    public class OrderService(ICommandBus commandBus, IOrdersRepository ordersRepository, IMapper mapper) : IOrderService
    {
        private readonly ICommandBus _commandBus = commandBus;
        private readonly IOrdersRepository _ordersRepository = ordersRepository;
        private readonly IMapper _mapper = mapper;
        public async Task Create(NewOrder newOrderDto)
        {
            var verifyProductCommand = new VerifyProductCommand(newOrderDto.CustomerId, newOrderDto.ProductId, newOrderDto.Quantity);
            await _commandBus.SendCommand(verifyProductCommand);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _ordersRepository.DeleteAsync(id);
        }

        public async Task<GetOrder> GetByIdAsync(int id)
        {
            var order = await _ordersRepository.GetByIdAsync(id);
            return _mapper.Map<GetOrder>(order);
        }

        public async Task<GetOrder> Update(int id, UpdateOrder updateOrder)
        {
            var existingOrder = await _ordersRepository.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException($"Order with id {id} not found.");

            var order = _mapper.Map(updateOrder, existingOrder);
            await _ordersRepository.UpdateAsync(order);
            return _mapper.Map<GetOrder>(order);
        }
    }
}

using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Orders.Domain.Commands;
using CoE.Assessment.Orders.Domain.Models;
using CoE.Assessment.Orders.Domain.Repositories;

namespace CoE.Assessment.Orders.Domain.CommandHandlers
{
    public class CreateOrderCommandHandler(IOrdersRepository ordersRepository) : ICommandHandler<CreateOrderCommand>
    {
        private readonly IOrdersRepository _ordersRepository = ordersRepository;

        public async Task Handle(CreateOrderCommand message)
        {
            var order = new Order
            {
                CustomerId = message.CustomerId,
                ProductId = message.ProductId,
                Quantity = message.Quantity,
                TotalAmount = message.TotalAmount
            };
            await _ordersRepository.CreateAsync(order);
        }
    }
}

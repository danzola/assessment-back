using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Domain.Events;
using CoE.Assessment.Products.Domain.Commands;
using CoE.Assessment.Products.Domain.Events;
using CoE.Assessment.Products.Domain.Repositories;

namespace CoE.Assessment.Products.Domain.CommandHandlers
{
    public class VerifyProductCommandHandler(ICommandBus commandBus, IEventBus eventBus, IProductsRepository productsRepository) : ICommandHandler<VerifyProductCommand>
    {
        private readonly ICommandBus _commandBus = commandBus;
        private readonly IEventBus _eventBus = eventBus;
        private readonly IProductsRepository _productsRepository = productsRepository;
        public async Task Handle(VerifyProductCommand verifyProductCommand)
        {
            var product = await _productsRepository.GetByIdAsync(verifyProductCommand.ProductId);
            if(product != null)
            {
                decimal totalAmount = product.Price * verifyProductCommand.Quantity;
                var createOrderCommand = new CreateOrderCommand(verifyProductCommand.CustomerId, verifyProductCommand.ProductId, verifyProductCommand.Quantity, totalAmount);
                await _commandBus.SendCommand(createOrderCommand);
            }
            else
            {
                string message = $"Product with id {verifyProductCommand.ProductId} not exists";
                await _eventBus.PublishEvent(new LogErrorEvent(message));
            }
        }
    }
}

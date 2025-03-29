using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Products.Domain.Commands;
using CoE.Assessment.Products.Domain.Repositories;

namespace CoE.Assessment.Products.Domain.CommandHandlers
{
    public class VerifyProductCommandHandler(ICommandBus commandBus, IProductsRepository productsRepository) : ICommandHandler<VerifyProductCommand>
    {
        private readonly ICommandBus _commandBus = commandBus;
        private readonly IProductsRepository _productsRepository = productsRepository;
        public async Task Handle(VerifyProductCommand message)
        {
            var product = await _productsRepository.GetByIdAsync(message.ProductId);
            if(product != null)
            {
                decimal totalAmount = product.Price * message.Quantity;
                var createOrderCommand = new CreateOrderCommand(message.CustomerId, message.ProductId, message.Quantity, totalAmount);
                await _commandBus.SendCommand(createOrderCommand);
            }
            else
            {
                throw new NotImplementedException("Product not exists");
            }
        }
    }
}

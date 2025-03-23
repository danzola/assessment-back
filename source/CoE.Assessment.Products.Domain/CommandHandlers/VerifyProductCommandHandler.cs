using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Products.Domain.Commands;

namespace CoE.Assessment.Products.Domain.CommandHandlers
{
    public class VerifyProductCommandHandler(ICommandBus commandBus) : ICommandHandler<VerifyProductCommand>
    {
        private readonly ICommandBus _commandBus = commandBus;
        public async Task Handle(VerifyProductCommand message)
        {
            bool isAvailable = true;
            if(isAvailable)
            {
                decimal productPrice = 149.99m;
                decimal totalAmount = productPrice * message.Quantity;
                var createOrderCommand = new CreateOrderCommand(message.CustomerId, message.ProductId, message.Quantity, totalAmount);
                await _commandBus.SendCommand(createOrderCommand);
            }
        }
    }
}

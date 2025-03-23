using CoE.Assessment.Domain.Commands;
using CoE.Assessment.Products.Domain.Commands;

namespace CoE.Assessment.Products.Domain.CommandHandlers
{
    public class VerifyProductCommandHandler : ICommandHandler<VerifyProductCommand>
    {
        public async Task Handle(VerifyProductCommand message)
        {
            Console.WriteLine($"{message.ProductId}-{message.Quantity}-{message.MessageType}");
            await Task.CompletedTask;
        }
    }
}

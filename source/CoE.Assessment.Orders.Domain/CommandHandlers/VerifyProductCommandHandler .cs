using CoE.Assessment.Domain.Bus;
using CoE.Assessment.Orders.Domain.Commands;
using CoE.Assessment.Orders.Domain.Events;
using MediatR;

namespace CoE.Assessment.Orders.Domain.CommandHandlers
{
    public class VerifyProductCommandHandler(IEventBus eventBus) : IRequestHandler<VerifyProductCommand, bool>
    {
        private readonly IEventBus _eventBus = eventBus;

        public async Task<bool> Handle(VerifyProductCommand request, CancellationToken cancellationToken)
        {
            var productVerifiedEvent = new ProductVerifiedEvent(request.CustomerId, request.ProductId, request.Quantity);
            await _eventBus.Publish(productVerifiedEvent);
            return true;
        }
    }
}

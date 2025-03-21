using CoE.Assessment.Domain.Commands;

namespace CoE.Assessment.Orders.Domain.Commands
{
    public class VerifyProductCommand(int customerId, int productId, int quantity) : Command
    {
        public int CustomerId { get; init; } = customerId;
        public int ProductId { get; init; } = productId;
        public int Quantity { get; init; } = quantity;
    }
}

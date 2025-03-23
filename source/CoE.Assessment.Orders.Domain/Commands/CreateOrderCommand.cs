using CoE.Assessment.Domain.Commands;

namespace CoE.Assessment.Orders.Domain.Commands
{
    public class CreateOrderCommand(int customerId, int productId, int quantity, decimal totalAmount) : Command
    {
        public int CustomerId { get; init; } = customerId;
        public int ProductId { get; init; } = productId;
        public int Quantity { get; init; } = quantity;
        public decimal TotalAmount { get; init; } = totalAmount;
    }
}

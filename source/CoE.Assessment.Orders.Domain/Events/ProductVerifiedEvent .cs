using CoE.Assessment.Domain.Events;

namespace CoE.Assessment.Orders.Domain.Events
{
    public class ProductVerifiedEvent(int customerId, int productId, int quantity) : Event
    {
        public int CustomerId { get; init; } = customerId;
        public int ProductId { get; init; } = productId;
        public int Quantity { get; init; } = quantity;
    }
}

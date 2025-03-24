namespace CoE.Assessment.Orders.Application.Models
{
    public class GetOrder
    {
        public int Id { get; init; }
        public int CustomerId { get; init; }
        public int ProductId { get; init; }
        public int Quantity { get; init; }
        public decimal TotalAmount { get; init; }
        public DateTime OrderDate { get; init; }
    }
}

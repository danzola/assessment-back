namespace CoE.Assessment.Products.Application.Models
{
    public class GetProduct
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
    }
}

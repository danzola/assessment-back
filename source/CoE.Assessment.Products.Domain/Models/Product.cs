namespace CoE.Assessment.Products.Domain.Models
{
    public class Product
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
    }
}

namespace CoE.Assessment.Customers.Domain.Models
{
    public class Customer
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string Email { get; init; } = default!;
    }
}

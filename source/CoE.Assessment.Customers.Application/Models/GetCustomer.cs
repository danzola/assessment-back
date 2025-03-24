namespace CoE.Assessment.Customers.Application.Models
{
    public class GetCustomer
    {
        public int Id { get; init; }
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string Email { get; init; } = default!;
    }
}

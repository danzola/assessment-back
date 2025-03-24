using CoE.Assessment.Customers.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoE.Assessment.Customers.Data.Contexts
{
    public class CustomersDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
    }
}

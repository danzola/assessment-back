using CoE.Assessment.Orders.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoE.Assessment.Orders.Data.Contexts
{
    public class OrdersDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
    }
}

using CoE.Assessment.Products.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CoE.Assessment.Products.Data.Contexts
{
    public class ProductsDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}

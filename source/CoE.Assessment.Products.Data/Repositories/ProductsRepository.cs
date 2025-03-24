using CoE.Assessment.Products.Data.Contexts;
using CoE.Assessment.Products.Domain.Models;
using CoE.Assessment.Products.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoE.Assessment.Products.Data.Repositories
{
    public class ProductsRepository(ProductsDbContext context) : IProductsRepository
    {
        private readonly ProductsDbContext _context = context;
        public async Task<IEnumerable<Product>?> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}

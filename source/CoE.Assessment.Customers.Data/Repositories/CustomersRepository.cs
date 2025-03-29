using CoE.Assessment.Customers.Data.Contexts;
using CoE.Assessment.Customers.Domain.Models;
using CoE.Assessment.Customers.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoE.Assessment.Customers.Data.Repositories
{
    public class CustomersRepository(CustomersDbContext context) : ICustomersRepository
    {
        private readonly CustomersDbContext _context = context;

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<IEnumerable<Customer>?> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }
    }
}

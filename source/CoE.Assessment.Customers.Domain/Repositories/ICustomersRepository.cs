using CoE.Assessment.Customers.Domain.Models;

namespace CoE.Assessment.Customers.Domain.Repositories
{
    public interface ICustomersRepository
    {
        Task<Customer?> GetByIdAsync(int id);
        Task<IEnumerable<Customer>?> GetAll();
    }
}

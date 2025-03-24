using CoE.Assessment.Customers.Application.Models;

namespace CoE.Assessment.Customers.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<GetCustomer> GetByIdAsync(int id);
    }
}

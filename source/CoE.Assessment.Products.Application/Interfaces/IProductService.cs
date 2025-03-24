using CoE.Assessment.Products.Application.Models;

namespace CoE.Assessment.Products.Application.Interfaces
{
    public interface IProductService
    {
        Task<GetProduct> GetByIdAsync(int id);
        Task<IEnumerable<GetProduct>> GetAll();
    }
}

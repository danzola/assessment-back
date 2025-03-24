using CoE.Assessment.Products.Domain.Models;

namespace CoE.Assessment.Products.Domain.Repositories
{
    public interface IProductsRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>?> GetAll();
    }
}

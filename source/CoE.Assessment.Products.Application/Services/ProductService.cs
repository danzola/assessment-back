using AutoMapper;
using CoE.Assessment.Products.Application.Interfaces;
using CoE.Assessment.Products.Application.Models;
using CoE.Assessment.Products.Domain.Repositories;

namespace CoE.Assessment.Products.Application.Services
{
    public class ProductService(IProductsRepository productsRepository, IMapper mapper) : IProductService
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<GetProduct>> GetAll()
        {
            var products = await _productsRepository.GetAll();
            return _mapper.Map<IEnumerable<GetProduct>>(products);
        }

        public async Task<GetProduct> GetByIdAsync(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);
            return _mapper.Map<GetProduct>(product);
        }
    }
}

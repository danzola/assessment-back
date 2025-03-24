using AutoMapper;
using CoE.Assessment.Customers.Application.Interfaces;
using CoE.Assessment.Customers.Application.Models;
using CoE.Assessment.Customers.Domain.Repositories;

namespace CoE.Assessment.Customers.Application.Services
{
    public class CustomerService(ICustomersRepository customersRepository, IMapper mapper) : ICustomerService
    {
        private readonly ICustomersRepository _customersRepository = customersRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GetCustomer> GetByIdAsync(int id)
        {
            var customer = await _customersRepository.GetByIdAsync(id);
            return _mapper.Map<GetCustomer>(customer);
        }
    }
}

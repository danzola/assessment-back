using AutoMapper;
using CoE.Assessment.Customers.Application.Models;
using CoE.Assessment.Customers.Domain.Models;

namespace CoE.Assessment.Customers.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Customer, GetCustomer>();
        }
    }
}

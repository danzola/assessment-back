using AutoMapper;
using CoE.Assessment.Products.Application.Models;
using CoE.Assessment.Products.Domain.Models;

namespace CoE.Assessment.Products.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, GetProduct>();
        }
    }
}

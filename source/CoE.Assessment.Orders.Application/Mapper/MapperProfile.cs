using AutoMapper;
using CoE.Assessment.Orders.Application.Models;
using CoE.Assessment.Orders.Domain.Models;

namespace CoE.Assessment.Orders.Application.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile() 
        {
            CreateMap<Order, GetOrder>();
            CreateMap<UpdateOrder, Order>();
        }
    }
}

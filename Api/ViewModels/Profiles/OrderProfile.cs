using Api.ViewModels.Requests;
using Api.ViewModels.Responses;
using AutoMapper;
using Domain.Models;

namespace Api.ViewModels.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderRequest, Order>().ForMember(o => o.DeliveryTime, obj => obj.MapFrom(vm => vm.DeliveryTime.DateTime));
            CreateMap<Order, OrderResponse>();
        }
    }
}

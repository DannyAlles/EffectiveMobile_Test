using Api.ViewModels.Requests;
using Api.ViewModels.Responses;
using AutoMapper;
using Data.Models;
using System;

namespace Api.ViewModels.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderViewModel, Order>().ForMember(o => o.DeliveryTime, obj => obj.MapFrom(vm => vm.DeliveryTime.DateTime));
            CreateMap<Order, OrderViewModel>();
        }
    }
}

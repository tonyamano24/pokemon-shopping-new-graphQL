using AutoMapper;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;

namespace itsc_dotnet_practice.Models.Mapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto.OrderResponse>();
        CreateMap<OrderDetail, OrderDto.OrderDetailResponse>();
    }
}
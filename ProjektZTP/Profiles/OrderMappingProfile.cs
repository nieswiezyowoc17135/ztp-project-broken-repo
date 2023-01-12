using AutoMapper;
using ProjektZTP.Features.OrderFeatures.Commands;
using ProjektZTP.Models;

namespace ProjektZTP.Profiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<AddOrder.Command, Order>()
            .ForMember(dest => dest.ProductOrder,
                dto => dto.MapFrom(c => c.ProductIds.Select(x => new ProductOrder
            {
                ProductId = x
            }).ToList()));

    }
}


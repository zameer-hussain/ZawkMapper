using ZawkMapper.Configuration;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Mapping;

public sealed class OrderMappingProfile : MappingProfile
{
    public override void Configure(MapperConfiguration cfg)
    {
        cfg.ProjectModel<Order, OrderListDto>()
            .MapField(dest => dest.OrderId, src => src.Id)
            .MapField(dest => dest.OrderNumber, src => src.OrderNumber)
            .MapField(dest => dest.TotalAmount, src => src.TotalAmount)
            .MapField(dest => dest.OrderDateUtc, src => src.OrderDateUtc);
    }//Configure
}//OrderMappingProfile

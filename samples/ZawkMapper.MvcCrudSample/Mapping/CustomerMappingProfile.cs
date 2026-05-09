using ZawkMapper.Configuration;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Mapping;

public sealed class CustomerMappingProfile : MappingProfile
{
    public override void Configure(MapperConfiguration cfg)
    {
        cfg.MapModel<CustomerCreateDto, Customer>()
            .MapField(dest => dest.Status, src => CustomerStatus.Active);

        cfg.MapModel<CustomerEditDto, Customer>()
            .MapField(dest => dest.Id, src => src.CustomerId);

        cfg.ProjectModel<Customer, CustomerListDto>()
            .MapField(dest => dest.CustomerId, src => src.Id)
            .MapField(dest => dest.DisplayName, src => src.FullName)
            .MapField(dest => dest.StatusText, src =>
                src.Status == CustomerStatus.Active ? "Active" :
                src.Status == CustomerStatus.Inactive ? "Inactive" :
                src.Status == CustomerStatus.Suspended ? "Suspended" : "Unknown")
            .MapField(dest => dest.OrdersCount, src => src.Orders.Count(o => !o.IsDeleted))
            .MapField(dest => dest.TotalSpent, src => src.Orders.Where(o => !o.IsDeleted).Sum(o => (decimal?)o.TotalAmount) ?? 0m);

        cfg.ProjectModel<Customer, CustomerDetailsDto>()
            .MapField(dest => dest.CustomerId, src => src.Id)
            .MapField(dest => dest.StatusText, src =>
                src.Status == CustomerStatus.Active ? "Active" :
                src.Status == CustomerStatus.Inactive ? "Inactive" :
                src.Status == CustomerStatus.Suspended ? "Suspended" : "Unknown")
            .MapField(dest => dest.Orders, src => src.Orders
                .Where(o => !o.IsDeleted)
                .OrderByDescending(o => o.OrderDateUtc)
                .Take(10));

        cfg.ProjectModel<Customer, CustomerEditDto>("Edit")
            .MapField(dest => dest.CustomerId, src => src.Id);
    }//Configure
}//CustomerMappingProfile

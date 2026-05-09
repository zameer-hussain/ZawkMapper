using ZawkMapper.MvcCrudSample.Dtos;

namespace ZawkMapper.MvcCrudSample.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomerDetailsDto?> GetDetailsAsync(long id, CancellationToken cancellationToken = default);
    Task<CustomerEditDto?> GetForEditAsync(long id, CancellationToken cancellationToken = default);
    Task CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(CustomerEditDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}

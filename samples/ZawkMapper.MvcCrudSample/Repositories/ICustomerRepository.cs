using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Repositories;

public interface ICustomerRepository
{
    IQueryable<Customer> Query();
    Task<Customer?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<int> UpdateUsingSqlAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<int> SoftDeleteUsingSqlAsync(long id, CancellationToken cancellationToken = default);
}

using Microsoft.EntityFrameworkCore;
using ZawkMapper.Abstractions;
using ZawkMapper.Extensions;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Mapping;
using ZawkMapper.MvcCrudSample.Models;
using ZawkMapper.MvcCrudSample.Repositories;

namespace ZawkMapper.MvcCrudSample.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IObjectMapper _mapper;

    public CustomerService(
        ICustomerRepository repository,
        IObjectMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CustomerListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        //projection usage: db model to dto directly at database query level
        return await _repository.Query()
            .OrderByDescending(x => x.Id)
            .Take(200)
            .ProjectAs<CustomerListDto>(AppMappingConfig.StaticConfigMethod())
            .ToListAsync(cancellationToken);
    }

    public Task<CustomerDetailsDto?> GetDetailsAsync(long id, CancellationToken cancellationToken = default)
    {
        return _repository.Query()
            .Where(x => x.Id == id)
            .ProjectAs<CustomerDetailsDto>(AppMappingConfig.StaticConfigMethod())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<CustomerEditDto?> GetForEditAsync(long id, CancellationToken cancellationToken = default)
    {
        return _repository.Query()
            .Where(x => x.Id == id)
            .ProjectAs<CustomerEditDto>(AppMappingConfig.StaticConfigMethod(), "Edit")
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default)
    {
        //runtime mapping usage: dto to db model
        var entity = _mapper.Map<CustomerCreateDto, Customer>(dto)!;
        entity.PublicId = Guid.NewGuid();
        entity.CreatedAtUtc = DateTime.UtcNow;
        await _repository.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> UpdateAsync(CustomerEditDto dto, CancellationToken cancellationToken = default)
    {
        //runtime mapping prepares a db model, but actual update is raw parameterized sql in repository
        var entity = _mapper.Map<CustomerEditDto, Customer>(dto)!;
        entity.Id = dto.CustomerId;
        entity.UpdatedAtUtc = DateTime.UtcNow;

        var affected = await _repository.UpdateUsingSqlAsync(entity, cancellationToken);
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        //delete is also raw parameterized sql in repository
        var affected = await _repository.SoftDeleteUsingSqlAsync(id, cancellationToken);
        return affected > 0;
    }
}

# Getting started

ZawkMapper maps objects in memory and projects database queries into DTOs.

Install:

```bash
dotnet add package ZawkMapper
```

## Runtime mapping

Use `MapModel` when the source object is already loaded in memory.

```csharp
using ZawkMapper.Configuration;
using ZawkMapper.Core;

var config = new MapperConfiguration(cfg =>
{
    cfg.MapModel<Customer, CustomerDto>()
        .MapFieldStrict(d => d.Id, s => s.Id)
        .MapFieldStrict(d => d.Name, s => s.Name)
        .MapField(d => d.BalanceText, s => s.Balance);
});

var mapper = new ObjectMapper(config);
var dto = mapper.Map<Customer, CustomerDto>(customer);
```

## Projection

Use `ProjectModel` and `ProjectAs` when you want a query provider such as EF Core to select DTO fields.

```csharp
using ZawkMapper.Configuration;
using ZawkMapper.Extensions;

var config = new MapperConfiguration(cfg =>
{
    cfg.ProjectModel<Customer, CustomerDto>()
        .MapField(d => d.Id, s => s.Id)
        .MapField(d => d.Name, s => s.Name);
});

var customers = await db.Customers
    .ProjectAs<CustomerDto>(config)
    .ToListAsync();
```

## Rule of thumb

| Need | Use |
|---|---|
| map loaded objects | `MapModel` |
| project database query | `ProjectModel` + `ProjectAs` |
| same-type runtime fields | `MapFieldStrict` |
| assignable runtime fields | `MapFieldDirect` |
| conversion, computed fields, nested maps | `MapField` |

## Reuse configuration

Create mapping configuration once and reuse it. In ASP.NET Core, use dependency injection. In small apps, use a static cached configuration.

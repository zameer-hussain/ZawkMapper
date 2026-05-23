<p align="center">
  <img src="assets/zawkmapper-logo.png" alt="ZawkMapper Logo" width="720" />
</p>

<h1 align="center">ZawkMapper</h1>

<p align="center">
  Fast .NET object mapper, C# DTO mapping library, AutoMapper alternative, and SQL-friendly EF Core IQueryable projection by <strong>ZawkTech</strong>.
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/ZawkMapper/">NuGet</a> ·
  <a href="https://github.com/zameer-hussain/ZawkMapper">GitHub</a> ·
  <a href="https://github.com/zameer-hussain/ZawkMapper.Benchmarks">Benchmarks</a> ·
  <a href="https://www.zawktech.com/zawkmapper">Website</a>
</p>

<p align="center">
  Developer and founder: <strong>Zameer Hussain Vighio</strong>
  <br />
  Co-developer and contributor: <strong>Mr Aqib Ali Abbasi</strong>
</p>

## What is ZawkMapper?

ZawkMapper is a lightweight .NET object mapper and EF Core projection library for clean DTO mapping, runtime object mapping, nested object mapping, collection mapping, and SQL-friendly IQueryable projection.

It is built for developers who want simple mapping configuration with readable APIs such as `MapModel`, `ProjectModel`, `MapField`, `MapFieldStrict`, `MapFieldDirect`, and `ProjectAs`.

Use ZawkMapper when you need:

1. Entity to DTO mapping
2. DTO to entity mapping
3. Runtime object mapping
4. SQL-friendly EF Core projection
5. IQueryable projection with `ProjectAs`
6. Named mappings and named projections
7. Nested object and collection mapping
8. Strict, direct, and flexible field mapping
9. Cached configuration for application performance

## Install

```bash
dotnet add package ZawkMapper --version 0.6.1
```

## Quick runtime mapping

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.MapModel<Customer, CustomerDto>()
        .MapFieldStrict(d => d.Id, s => s.Id)
        .MapFieldDirect(d => d.Name, s => s.Name)
        .MapField(d => d.TotalText, s => s.Total);
});

var mapper = new ObjectMapper(config);
var dto = mapper.Map<Customer, CustomerDto>(customer);
```

## Quick EF Core projection

```csharp
var customers = await db.Customers
    .ProjectAs<CustomerDto>(config)
    .ToListAsync();
```

## Runtime mapping vs projection

ZawkMapper separates runtime object mapping and database projection.

| Scenario | API |
|---|---|
| Map objects already loaded in memory | `MapModel` |
| Project database queries into DTOs | `ProjectModel` and `ProjectAs` |

This lets runtime mapping use normal C# logic while projection remains SQL-friendly for EF Core and other LINQ providers.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => SomeNormalCSharpMethod(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

`ProjectAs` uses `ProjectModel` first. If no projection exists, it can reuse `MapModel` when the mapping rules are projection-safe.

## Field mapping options

| Method | Best for |
|---|---|
| `MapFieldStrict` | Same-type member mapping with compile-time safety |
| `MapFieldDirect` | Direct assignment when source and destination types already match |
| `MapField` | Flexible conversion, computed fields, nested object mapping, and collection bridges |

Example:

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapFieldStrict(d => d.Id, s => s.Id)
    .MapFieldDirect(d => d.Code, s => s.Code)
    .MapField(d => d.PriceText, s => s.Price);
```

## Strict mapping and nested collections

`MapFieldStrict` is for same source and destination member types. It is ideal for simple fields such as `Id`, `Name`, `Price`, `Quantity`, and `CreatedAtUtc`.

```csharp
cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity)
    .MapFieldStrict(d => d.UnitPrice, s => s.UnitPrice);
```

For a collection bridge such as `List<OrderItem>` to `List<OrderLineDto>`, use `MapField` on the parent collection member and define a child map separately.

```csharp
cfg.MapModel<Order, OrderDetailDto>()
    .MapField(d => d.Lines, s => s.Items);

cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity);
```

## ASP.NET Core setup

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(CustomerMappingProfile).Assembly);
});
```

`AddZawkMapper` registers `MapperConfiguration` as singleton and `IObjectMapper` as scoped.

## Profiles

```csharp
public sealed class CustomerMappingProfile : MappingProfile
{
    public override void Configure(MappingConfigurationExpression cfg)
    {
        cfg.MapModel<CustomerCreateDto, Customer>()
            .MapField(d => d.FullName, s => s.FullName)
            .MapField(d => d.IsPremium, s => s.IsPremium);

        cfg.ProjectModel<Customer, CustomerListDto>()
            .MapField(d => d.CustomerId, s => s.Id)
            .MapField(d => d.DisplayName, s => s.FullName)
            .MapField(d => d.OrdersCount, s => s.Orders.Count());
    }//Configure
}//CustomerMappingProfile
```

## Named projections

Use named projections when the same source and destination need different output rules.

```csharp
public static class ProductProjectionNames
{
    public const string English = "English";
    public const string Sindhi = "Sindhi";
}//ProductProjectionNames
```

```csharp
cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.English)
    .MapField(d => d.Name, s => s.NameEn)
    .MapField(d => d.Description, s => s.DescriptionEn);

cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.Sindhi)
    .MapField(d => d.Name, s => s.NameSd)
    .MapField(d => d.Description, s => s.DescriptionSd);
```

```csharp
var projectionName = isSindhi
    ? ProductProjectionNames.Sindhi
    : ProductProjectionNames.English;

var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig, projectionName)
    .ToListAsync();
```

ZawkMapper does not include built-in language enums. Projection names are your project choice. Constants help avoid spelling mistakes.

## Request-level projection values

Scenario-level configuration is valid when a projection expression depends on request values such as salary increment percentage, discount percentage, prefix, suffix, tenant-specific value, or request-based display text.

```csharp
public static MapperConfiguration CreateSalaryProjectionConfig(decimal incrementPercent)
{
    return new MapperConfiguration(cfg =>
    {
        cfg.ProjectModel<Employee, EmployeeSalaryDto>()
            .MapField(d => d.Id, s => s.Id)
            .MapField(d => d.FullName, s => s.FullName)
            .MapField(d => d.ProjectedSalary, s => s.Salary + (s.Salary * incrementPercent / 100));
    });
}//CreateSalaryProjectionConfig
```

For reusable rules, cached app-level configuration is usually better. For request-specific rules, scenario-level configuration gives more flexibility.

## Performance notes

ZawkMapper 0.6.1 promotes the tested 0.6.1 release candidate improvements to stable.

This release improves common runtime mapping paths, lowers allocation in same-type and direct mapping scenarios, and keeps `ProjectAs` projection behavior stable. Public benchmark results are available in the ZawkMapper benchmark repository.

Benchmark repository:

```text
https://github.com/zameer-hussain/ZawkMapper.Benchmarks
```

Honest performance guidance:

1. Manual mapping remains the best baseline for raw hand-written runtime speed.
2. ZawkMapper is competitive in flat DTO and summary DTO runtime mapping scenarios.
3. `ProjectAs` is strong for EF Core IQueryable projection scenarios.
4. Nested runtime collection mapping remains a future optimization target.
5. Results can vary by hardware, data shape, database provider, .NET runtime, and background workload.

## Compatibility aliases

ZawkMapper has preferred API names and migration-friendly aliases.

Preferred style:

```text
MapModel
ProjectModel
MapField
ProjectAs
```

Compatibility aliases:

```text
CreateMap
CreateProjection
ForMember
ForMemberStrict
ForMemberDirect
ProjectTo
```

## Documentation

Start here:

1. `docs/getting-started.md`
2. `docs/projection.md`
3. `docs/runtime-vs-projection.md`
4. `docs/real-world-scenarios.md`
5. `docs/dependency-injection.md`
6. `docs/performance.md`
7. `docs/0.6.1-notes.md`

## Supported targets

| Target |
|---|
| `netstandard2.1` |
| `net6.0` |
| `net8.0` |
| `net10.0` |

## Links

| Link | URL |
|---|---|
| NuGet | `https://www.nuget.org/packages/ZawkMapper/` |
| Benchmark repo | `https://github.com/zameer-hussain/ZawkMapper.Benchmarks` |
| Website | `https://www.zawktech.com/zawkmapper` |
| LinkedIn | `https://www.linkedin.com/company/zawktech` |
| Developer | `https://www.linkedin.com/in/zameer-vighio/` |

## License

MIT

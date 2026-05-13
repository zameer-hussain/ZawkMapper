<p align="center">
  <img src="assets/zawkmapper-logo.png" alt="ZawkMapper Logo" width="720" />
</p>

<h1 align="center">ZawkMapper</h1>

<p align="center">
  Fast object mapping and SQL-friendly projection for .NET.
</p>

<p align="center">
  Developer and founder: Zameer Hussain Vighio.
  <br />
  Co-developer and contributor: Mr Aqib Ali Abbasi.
</p>

## Install

```bash
dotnet add package ZawkMapper --prerelease
```

## Quick use

Register maps and projections:

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.MapModel<CustomerCreateDto, Customer>()
        .MapField(d => d.FullName, s => s.FullName)
        .MapField(d => d.IsPremium, s => s.IsPremium);

    cfg.ProjectModel<Customer, CustomerListDto>()
        .MapField(d => d.CustomerId, s => s.Id)
        .MapField(d => d.DisplayName, s => s.FullName)
        .MapField(d => d.OrdersCount, s => s.Orders.Count());
});
```

Runtime mapping:

```csharp
var mapper = new ObjectMapper(config);
var customer = mapper.Map<CustomerCreateDto, Customer>(request);
```

Projection:

```csharp
var list = await db.Customers
    .ProjectAs<CustomerListDto>(config)
    .ToListAsync();
```

## ASP.NET Core registration

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(CustomerMappingProfile).Assembly);
});
```

`AddZawkMapper` registers `MapperConfiguration` as singleton and `IObjectMapper` as scoped.

You do not need to manually add `IObjectMapper` when `AddZawkMapper` is used.

## Passing configuration to ProjectAs

Both forms are supported:

```csharp
.ProjectAs<ProductDto>(_mapperConfig)
```

```csharp
.ProjectAs<ProductDto>(AppMappingConfig.StaticConfigMethod())
```

The second form is good when `StaticConfigMethod()` returns the same cached `MapperConfiguration` object.

## Runtime map and projection can be different

This is useful when runtime mapping uses normal C# code but projection must stay SQL friendly.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => SomeNormalCSharpMethod(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

`Map` uses `MapModel`.

`ProjectAs` uses `ProjectModel`.

If `ProjectModel` does not exist, `ProjectAs` can reuse `MapModel` when the rules are projection-safe.

## Named projections

Keep projection names in constants to avoid spelling mistakes.

```csharp
public static class ProductProjectionNames
{
    public const string English = "English";
    public const string Sindhi = "Sindhi";
}//ProductProjectionNames
```

```csharp
cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.English)
    .MapField(d => d.Name, s => s.NameEn);

cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.Sindhi)
    .MapField(d => d.Name, s => s.NameSd);
```

```csharp
var projectionName = isSindhi
    ? ProductProjectionNames.Sindhi
    : ProductProjectionNames.English;

var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig, projectionName)
    .ToListAsync();
```

The package does not include built-in language enums. Names are your project choice.

## Request-level projection values

For values that come from the current request, scenario-level configuration is valid.

```csharp
public static MapperConfiguration CreateSalaryProjectionConfig(decimal incrementPercent)
{
    return new MapperConfiguration(cfg =>
    {
        cfg.ProjectModel<Employee, EmployeeSalaryDto>()
            .MapField(d => d.ProjectedSalary, s => s.Salary + (s.Salary * incrementPercent / 100));
    });
}//CreateSalaryProjectionConfig
```

Reusable app-level configuration is usually faster. Scenario-level configuration is useful when the mapping rule itself depends on a runtime value.


## EF Core global filters

If an entity already has a global query filter, avoid repeating the same condition inside `ProjectModel` unless you need it there on purpose.

For example, if `Order` already uses `HasQueryFilter(x => !x.IsDeleted)`, use:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count())
```

This keeps the projection clean while EF Core still applies the soft delete filter when SQL is generated.

## Docs

Start here:

1. `docs/getting-started.md`
2. `docs/projection.md`
3. `docs/runtime-vs-projection.md`
4. `docs/real-world-scenarios.md`
5. `docs/dependency-injection.md`
6. `docs/performance.md`
7. `docs/0.6.0-rc.2-notes.md`

## License

MIT

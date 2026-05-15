<p align="center">
  <img src="assets/zawkmapper-logo.png" alt="ZawkMapper Logo" width="720" />
</p>

<h1 align="center">ZawkMapper</h1>

<p align="center">
  Fast object mapping and SQL-friendly projection for .NET.
</p>

<p align="center">
  Built under <strong>ZawkTech</strong>
  <br />
  Developer and founder: <strong>Zameer Hussain Vighio</strong>
  <br />
  Co-developer and contributor: <strong>Mr Aqib Ali Abbasi</strong>
</p>

## What is ZawkMapper?

ZawkMapper is a .NET mapping package for real project scenarios.

It helps you map objects in memory and project database queries into DTOs with clean, readable configuration.

Use it when you need:

1. Entity to DTO mapping
2. DTO to entity mapping
3. SQL-friendly `IQueryable` projection
4. Named maps and named projections
5. Nested object and collection mapping
6. Strict, direct, and flexible field mapping
7. Cached configuration for better application performance

## Install

```bash
dotnet add package ZawkMapper
```

## Basic idea

ZawkMapper has two main jobs.

| Use case | Method |
|---|---|
| Runtime object mapping | `MapModel` |
| Database query projection | `ProjectModel` |

Runtime mapping is useful when data is already in memory.

Projection is useful when you want EF Core or another query provider to select DTO fields directly from the database.

## Quick example

Register mapping rules once:

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

Map an object in memory:

```csharp
var mapper = new ObjectMapper(config);

var customer = mapper.Map<CustomerCreateDto, Customer>(request);
```

Project a query into a DTO:

```csharp
var customers = await db.Customers
    .ProjectAs<CustomerListDto>(config)
    .ToListAsync();
```

## ASP.NET Core setup

Register ZawkMapper in `Program.cs`:

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(CustomerMappingProfile).Assembly);
});
```

`AddZawkMapper` registers:

| Service | Lifetime |
|---|---|
| `MapperConfiguration` | Singleton |
| `IObjectMapper` | Scoped |

You do not need to manually register `IObjectMapper` when `AddZawkMapper` is used.

## Using profiles

Keep mapping rules in profile classes instead of putting everything in `Program.cs`.

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

## Passing configuration to ProjectAs

You can pass an injected configuration object:

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig)
    .ToListAsync();
```

You can also use a static cached method:

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(AppMappingConfig.StaticConfigMethod())
    .ToListAsync();
```

Example cached configuration:

```csharp
public static class AppMappingConfig
{
    private static readonly Lazy<MapperConfiguration> Cached = new(() =>
        new MapperConfiguration(cfg =>
        {
            cfg.AddProfilesFromAssembly(typeof(AppMappingConfig).Assembly);
        }));

    public static MapperConfiguration StaticConfigMethod()
    {
        return Cached.Value;
    }//StaticConfigMethod
}//AppMappingConfig
```

Both styles are valid. The important point is to reuse configuration instead of rebuilding it again and again.

## Runtime mapping and projection can be different

Sometimes runtime mapping can use normal C# code, but database projection must stay SQL-friendly.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => SomeNormalCSharpMethod(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

In this case:

| Operation | Uses |
|---|---|
| `Map` | `MapModel` |
| `ProjectAs` | `ProjectModel` |

If `ProjectModel` is not registered, `ProjectAs` can reuse `MapModel` when the rules are safe for projection.

## Field mapping options

ZawkMapper gives you three field mapping styles.

| Method | Best for |
|---|---|
| `MapFieldStrict` | Same-type mapping with compile-time safety |
| `MapFieldDirect` | Fast direct assignment when types already match |
| `MapField` | Flexible runtime conversion when needed |

Example:

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapFieldStrict(d => d.Id, s => s.Id)
    .MapFieldDirect(d => d.Code, s => s.Code)
    .MapField(d => d.PriceText, s => s.Price);
```

## Named projections

Use named projections when the same source and destination need different output rules.

A common example is language-based fields.

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

ZawkMapper does not include built-in language enums. Projection names are your project choice.

Using constants helps your team avoid spelling mistakes.

## Request-level projection values

Sometimes projection rules depend on the current request.

Examples:

1. Salary increment percentage
2. Discount percentage
3. Prefix or suffix
4. Tenant-specific value
5. Request-based display text

For those cases, scenario-level configuration is valid.

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

```csharp
var config = EmployeeMappingConfig.CreateSalaryProjectionConfig(request.IncrementPercent);

var employees = await db.Employees
    .ProjectAs<EmployeeSalaryDto>(config)
    .ToListAsync();
```

For reusable rules, cached app-level configuration is usually better.

For request-specific rules, scenario-level configuration gives more flexibility.

## EF Core global filters

If an entity already has an EF Core global query filter, avoid repeating the same condition inside `ProjectModel` unless you need it there on purpose.

Example:

```csharp
modelBuilder.Entity<Order>()
    .HasQueryFilter(x => !x.IsDeleted);
```

Use:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count())
```

Instead of:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count(x => !x.IsDeleted))
```

EF Core will apply the global filter when SQL is generated.

## Compatibility aliases

ZawkMapper has its own preferred API names.

Preferred style:

```csharp
MapModel
ProjectModel
MapField
ProjectAs
```

Compatibility aliases are also available for developers coming from other mapping libraries:

```csharp
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
7. `docs/0.6.0-notes.md`

For stable release details, read:

```text
docs/0.6.0-notes.md
```

## Supported targets

ZawkMapper supports:

| Target |
|---|
| `netstandard2.1` |
| `net6.0` |
| `net8.0` |
| `net10.0` |

## License

MIT
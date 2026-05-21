# ZawkMapper

**ZawkMapper** is a lightweight .NET object mapper and EF Core projection library for clean DTO mapping, fast runtime object mapping, nested mapping, collection mapping, and SQL-friendly `IQueryable` projection.

It is built for developers who want simple mapping configuration without giving up projection support.

<p align="center">
  <img src="assets/zawkmapper-logo.png" alt="ZawkMapper Logo" width="520" />
</p>

<p align="center">
  <strong>ZawkTech</strong><br />
  Developer and founder: <strong>Zameer Hussain Vighio</strong><br />
  Co-developer and contributor: <strong>Mr Aqib Ali Abbasi</strong>
</p>

## Install

```bash
dotnet add package ZawkMapper
```

## Quick runtime mapping

```csharp
using ZawkMapper.Configuration;
using ZawkMapper.Core;

var config = new MapperConfiguration(cfg =>
{
    cfg.MapModel<Customer, CustomerDto>()
        .MapFieldStrict(d => d.Id, s => s.Id)
        .MapFieldStrict(d => d.Name, s => s.Name)
        .MapField(d => d.TotalText, s => s.Total);
});

var mapper = new ObjectMapper(config);
var dto = mapper.Map<Customer, CustomerDto>(customer);
```

## Quick EF Core projection

```csharp
using ZawkMapper.Configuration;
using ZawkMapper.Extensions;

var config = new MapperConfiguration(cfg =>
{
    cfg.ProjectModel<Customer, CustomerListDto>()
        .MapField(d => d.Id, s => s.Id)
        .MapField(d => d.Name, s => s.Name)
        .MapField(d => d.OrdersCount, s => s.Orders.Count());
});

var customers = await db.Customers
    .ProjectAs<CustomerListDto>(config)
    .ToListAsync();
```

## Main features

| Feature | Purpose |
|---|---|
| `MapModel` | Runtime object mapping |
| `ProjectModel` | SQL-friendly query projection |
| `ProjectAs` | Project `IQueryable` into DTOs |
| `MapFieldStrict` | Same-type mapping with compile-time safety |
| `MapFieldDirect` | Direct assignment when the value is assignable |
| `MapField` | Flexible conversion, computed values, nested maps, and collection bridges |
| Named maps/projections | Different output rules for list, detail, edit, language, public/admin screens |
| Cached configuration | Reuse mapping plans for better performance |

## Which field method should I use?

Use this rule first:

```text
same source/destination type      -> MapFieldStrict
assignable source/destination     -> MapFieldDirect
conversion/computed/nested bridge -> MapField
```

Example:

```csharp
cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity)
    .MapFieldStrict(d => d.UnitPrice, s => s.UnitPrice);
```

For a parent collection bridge, use `MapField` because the member types are different:

```csharp
cfg.MapModel<Order, OrderDetailDto>()
    .MapField(d => d.Lines, s => s.Items);

cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity)
    .MapFieldStrict(d => d.UnitPrice, s => s.UnitPrice);
```

`List<OrderItem>` and `List<OrderLineDto>` are not the same member type, so `MapFieldStrict` is not the right parent bridge.

## Runtime mapping and projection can be different

Runtime mapping can use normal C# code. Projection must stay provider-friendly so EF Core can translate it to SQL.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => FormatName(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

`ProjectAs` uses `ProjectModel` first. If no `ProjectModel` exists, it can fall back to `MapModel` when the mapping expression is projection-safe.

## ASP.NET Core setup

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(CustomerMappingProfile).Assembly);
});
```

This registers:

| Service | Lifetime |
|---|---|
| `MapperConfiguration` | Singleton |
| `IObjectMapper` | Scoped |

## Named projections example

Named projections are useful when the same entity has different output rules.

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

ZawkMapper keeps names generic. Use constants in your project to avoid spelling mistakes.

## Performance notes

ZawkMapper 0.6.1 focuses on runtime mapping speed, lower allocation, and stable `ProjectAs` projection behavior.

General guidance:

- reuse `MapperConfiguration`
- use dependency injection or static cached configuration
- use `MapFieldStrict` for same-type runtime fields
- use `MapField` only where conversion, computed values, nested maps, or collection bridges are needed
- use `ProjectAs` for database-backed lists and screens
- keep runtime mapping and EF projection benchmarks separate

Benchmarks depend on model shape, CPU, database provider, warmup, and query shape. Run your own benchmark for your own workload before making hard claims.

## Documentation

Public docs:

- `docs/public/getting-started.md`
- `docs/public/projection.md`
- `docs/public/runtime-vs-projection.md`
- `docs/public/performance.md`
- `docs/public/strict-mapping.md`
- `docs/public/direct-mapping.md`
- `docs/public/nested-mapping.md`
- `docs/public/dependency-injection.md`
- `docs/public/real-world-scenarios.md`

Owner/developer notes are kept separately under:

- `docs/private/`

## Supported targets

| Target |
|---|
| `netstandard2.1` |
| `net6.0` |
| `net8.0` |
| `net10.0` |

## License

MIT

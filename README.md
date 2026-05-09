<p align="center">
  <img src="assets/zawkmapper-logo.png" alt="ZawkMapper Logo" width="720" />
</p>

<h1 align="center">ZawkMapper</h1>

<p align="center">
  Fast, flexible, developer-friendly object mapping and SQL-friendly projection library for .NET.
</p>

<p align="center">
  Model mapping • DTO mapping • EF Core projection • nested objects • nested collections • strict mapping • runtime conversion
</p>

---

## What is ZawkMapper?

**ZawkMapper** is a .NET object mapper and projection library created for developers who want clean DTO mapping without heavy configuration noise.

It supports both styles developers usually need in real projects:

- **strict compile-time mapping** when source and destination member types must match
- **flexible runtime mapping** when safe conversion is useful, such as `string` to `long` or enum to string
- **SQL-friendly projection** for database queries using `ProjectModel` and `ProjectAs`

ZawkMapper is created and maintained by **Zameer Hussain Vighio** under **ZawkTech**.

---

## Install

If you are using Visual Studio, enable **Include prerelease** before searching for `ZawkMapper`.

```bash
dotnet add package ZawkMapper --prerelease
```

For a stable version later, use:

```bash
dotnet add package ZawkMapper
```

---

## Quick example

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.MapModel<CustomerCreateDto, Customer>()
       .MapFieldStrict(dest => dest.Email, src => src.Email)
       .MapField(dest => dest.IsPremium, src => src.IsPremium);

    cfg.ProjectModel<Customer, CustomerListDto>()
       .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
       .MapFieldStrict(dest => dest.DisplayName, src => src.FullName)
       .MapField(dest => dest.OrdersCount, src => src.Orders.Count(o => !o.IsDeleted));
});
```

Use runtime mapping:

```csharp
var mapper = new ObjectMapper(config);
var customer = mapper.Map<CustomerCreateDto, Customer>(request);
```

Use SQL-friendly projection:

```csharp
var list = db.Customers
    .Where(x => !x.IsDeleted)
    .ProjectAs<CustomerListDto>(config)
    .ToList();
```

---

## Strict mapping vs flexible mapping

ZawkMapper supports both flexible mapping and compile-time strict mapping.

### For maximum compile-time safety, use `MapFieldStrict`

```csharp
cfg.MapModel<Customer, CustomerDto>()
   .MapFieldStrict(dest => dest.Email, src => src.Email);
```

`MapFieldStrict` requires both sides to use the same member type. If the destination member is `long` and the source member is `string`, the code should not compile.

### For flexible DTO mapping, use `MapField`

```csharp
cfg.MapModel<CustomerCreateDto, Customer>()
   .MapField(dest => dest.IsPremium, src => src.IsPremium);
```

Runtime mapping can try safe conversions such as:

- `string` to `long`
- `string` to `bool`
- enum to `string`
- enum to numeric value
- numeric value to enum
- nullable and non-nullable compatible values

### For database queries, prefer SQL-friendly expressions in `ProjectModel`

```csharp
cfg.ProjectModel<Customer, CustomerListDto>()
   .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
   .MapField(dest => dest.StatusText, src =>
       src.Status == CustomerStatus.Active ? "Active" :
       src.Status == CustomerStatus.Inactive ? "Inactive" : "Unknown");
```

Projection should stay provider-friendly and avoid runtime-only conversions inside `IQueryable`.

---

## Nested projection

Define the child projection once:

```csharp
cfg.ProjectModel<Order, OrderListDto>()
   .MapFieldStrict(dest => dest.OrderId, src => src.Id)
   .MapFieldStrict(dest => dest.OrderNumber, src => src.OrderNumber)
   .MapFieldStrict(dest => dest.TotalAmount, src => src.TotalAmount);
```

Use it in the parent projection without writing manual `.Select(...)`:

```csharp
cfg.ProjectModel<Customer, CustomerDetailsDto>()
   .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
   .MapField(dest => dest.Orders, src => src.Orders
       .Where(o => !o.IsDeleted)
       .OrderByDescending(o => o.OrderDateUtc)
       .Take(10));
```

ZawkMapper will reuse the registered `Order -> OrderListDto` projection.

---

## Named projections

Names are optional. Use them only when the same source and destination pair has multiple mapping purposes.

```csharp
cfg.ProjectModel<Customer, CustomerDto>("Edit")
   .MapFieldStrict(dest => dest.CustomerId, src => src.Id);

cfg.ProjectModel<Customer, CustomerDto>("Detail")
   .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
   .MapField(dest => dest.Orders, src => src.Orders.Where(o => !o.IsDeleted));
```

Then select the projection:

```csharp
var editDto = db.Customers.ProjectAs<CustomerDto>(config, "Edit").FirstOrDefault();
```

If duplicate unnamed maps exist for the same source and destination pair, ZawkMapper throws a clear configuration error.

---

## Configuration styles

### Static cached configuration

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

### Dependency injection

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(CustomerMappingProfile).Assembly);
});
```

---

## Compatibility aliases

ZawkMapper has its own official naming style:

- `MapModel`
- `ProjectModel`
- `MapField`
- `ProjectAs`

Compatibility aliases are also available to make migration easier for existing projects:

- `CreateMap`
- `CreateProjection`
- `ForMember`
- `ProjectTo`

---

## Performance notes

For production web apps, create mapper configuration once and reuse it.

Good:

- one cached/singleton configuration
- cached projection expressions
- cached runtime delegates
- `ProjectAs` for database-backed list/detail screens

Avoid:

- creating configuration per request
- creating configuration per method call in hot paths
- doing runtime-only conversion inside database projections

---

## Documentation

- [Getting Started](docs/getting-started.md)
- [Strict Mapping](docs/strict-mapping.md)
- [Projection Guide](docs/projection.md)
- [Nested Mapping](docs/nested-mapping.md)
- [Performance Guide](docs/performance.md)
- [Testing Guide](docs/testing.md)
- [Publishing Guide](docs/PUBLISHING.md)

---

## Feedback and issues

If you find a bug or performance issue, please include:

- ZawkMapper version
- .NET version
- project type
- source/destination models
- mapping configuration
- generated SQL if EF Core projection is involved
- expected result
- actual result

---

## Author

Created and maintained by **Zameer Hussain Vighio** under **ZawkTech**.

- Website: www.zawktech.com
- Email: zameer.vighio@hotmail.com
- WhatsApp: +92 313 1311121
- LinkedIn: https://www.linkedin.com/in/zameer-vighio/
- GitHub: https://github.com/zameer-hussain


## Performance testing

ZawkMapper includes internal performance labs for release testing. These labs compare:

- strict `MapFieldStrict` mapping
- flexible `MapField` mapping
- mixed strict + flexible mapping
- cached runtime mapping plans
- cached projection expressions
- nested projection
- named projection
- memory delta per scenario

Recommended production usage remains simple:

```text
Use MapFieldStrict when you want maximum compile-time type safety.
Use MapField when flexible runtime conversion is helpful.
Use ProjectModel and ProjectAs for SQL-friendly database projection.
Use static cached or DI singleton configuration for web apps.
```


## Mapping safety modes

ZawkMapper gives you three clear mapping choices.

```csharp
cfg.MapModel<User, UserDto>()
   .MapFieldStrict(dest => dest.Email, src => src.Email)
   .MapFieldDirect(dest => dest.FullName, src => src.FullName)
   .MapField(dest => dest.StatusText, src => src.Status);
```

- `MapFieldStrict` is for compile-time same-type safety.
- `MapFieldDirect` is for direct assignment without conversion.
- `MapField` is for flexible runtime conversion.

For database query projection, prefer SQL-friendly expressions with `ProjectModel` and `ProjectAs`.

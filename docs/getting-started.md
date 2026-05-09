# Getting Started with ZawkMapper

ZawkMapper helps .NET developers map models, DTOs, and query projections with clean configuration.

## Install

For prerelease versions, enable **Include prerelease** in Visual Studio NuGet Package Manager.

```bash
dotnet add package ZawkMapper --prerelease
```

## Create configuration

```csharp
var config = new MapperConfiguration(cfg =>
{
    cfg.MapModel<CustomerCreateDto, Customer>()
       .MapFieldStrict(dest => dest.Email, src => src.Email)
       .MapField(dest => dest.IsPremium, src => src.IsPremium);

    cfg.ProjectModel<Customer, CustomerListDto>()
       .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
       .MapFieldStrict(dest => dest.DisplayName, src => src.FullName);
});
```

## Runtime mapping

```csharp
var mapper = new ObjectMapper(config);
var entity = mapper.Map<CustomerCreateDto, Customer>(dto);
```

## Query projection

```csharp
var list = db.Customers
    .ProjectAs<CustomerListDto>(config)
    .ToList();
```


## Strict, direct, and flexible mapping

```csharp
cfg.MapModel<User, UserDto>()
   .MapFieldStrict(dest => dest.Email, src => src.Email)
   .MapFieldDirect(dest => dest.FullName, src => src.FullName)
   .MapField(dest => dest.RoleName, src => src.Role);
```

- Use `MapFieldStrict` for compile-time same-type safety.
- Use `MapFieldDirect` for direct assignment without conversion.
- Use `MapField` when flexible conversion is intended.

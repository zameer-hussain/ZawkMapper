# Dependency injection

For ASP.NET Core apps, register ZawkMapper once in `Program.cs`.

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

Do not manually register `IObjectMapper` again when `AddZawkMapper` is already used.

## Profile example

```csharp
public sealed class CustomerMappingProfile : MappingProfile
{
    public override void Configure(MappingConfigurationExpression cfg)
    {
        cfg.MapModel<CustomerCreateDto, Customer>()
            .MapFieldStrict(d => d.FullName, s => s.FullName)
            .MapFieldStrict(d => d.IsPremium, s => s.IsPremium);

        cfg.ProjectModel<Customer, CustomerListDto>()
            .MapField(d => d.Id, s => s.Id)
            .MapField(d => d.FullName, s => s.FullName)
            .MapField(d => d.OrdersCount, s => s.Orders.Count());
    }//Configure
}//CustomerMappingProfile
```

# Performance guide

ZawkMapper performance depends on the mapping style, model shape, warmup, and whether the job is runtime mapping or database projection.

## Production setup

Recommended setup:

- create `MapperConfiguration` once
- use dependency injection or a static cached configuration
- use `ProjectAs` for database-backed screens
- use `MapFieldStrict` for same-type runtime fields
- use `MapField` only for conversion, computed values, nested maps, and collection bridges
- avoid rebuilding configuration inside loops

## Static cached configuration

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

## Field method guidance

| Method | Best use |
|---|---|
| `MapFieldStrict` | same source and destination member type |
| `MapFieldDirect` | assignable value, direct assignment style |
| `MapField` | conversion, computed values, nested maps, collection bridges |

Example:

```csharp
cfg.MapModel<Customer, CustomerDto>()
    .MapFieldStrict(d => d.Id, s => s.Id)
    .MapFieldStrict(d => d.Name, s => s.Name)
    .MapField(d => d.DisplayBalance, s => s.Balance);
```

## Nested collection bridge

Do not use `MapFieldStrict` for a collection bridge when source and destination member types are different.

```csharp
cfg.MapModel<Order, OrderDetailDto>()
    .MapField(d => d.Lines, s => s.Items);

cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity)
    .MapFieldStrict(d => d.UnitPrice, s => s.UnitPrice);
```

## Benchmark honestly

Keep runtime mapping and projection results separate.

Runtime mapping measures object-to-object mapping after data is already loaded.

Projection measures query expression translation and database/provider behavior.

Useful comparison groups:

- manual mapping
- AutoMapper runtime mapping
- ZawkMapper `MapField`
- ZawkMapper `MapFieldDirect`
- ZawkMapper `MapFieldStrict`
- ZawkMapper mixed mapping
- manual `Select`
- AutoMapper `ProjectTo`
- ZawkMapper `ProjectAs`

Do not claim one library is faster in every case. Model shape, nested collections, database provider, CPU load, and warmup all matter.

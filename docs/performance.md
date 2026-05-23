# ZawkMapper Performance Guide

ZawkMapper is a .NET object mapper and EF Core projection library built by ZawkTech for DTO mapping, runtime object mapping, SQL-friendly IQueryable projection, nested mapping, and collection mapping.

## Current stable version

```text
ZawkMapper 0.6.1
```

## Main performance guidance

Use the mapping method that fits the field.

| Method | Recommended use |
|---|---|
| `MapFieldStrict` | Same-type member mapping with compile-time safety |
| `MapFieldDirect` | Explicit direct same-type assignment |
| `MapField` | Flexible conversion, computed fields, nested objects, and collection bridges |

## Runtime mapping

Runtime mapping is used when objects are already loaded in memory.

```csharp
cfg.MapModel<Customer, CustomerDto>()
    .MapFieldStrict(d => d.Id, s => s.Id)
    .MapFieldDirect(d => d.Name, s => s.Name)
    .MapField(d => d.TotalText, s => s.Total);
```

Use cached application-level configuration where possible. Rebuilding mapping configuration repeatedly is slower than reusing a singleton or cached configuration.

## Projection

Projection is used when EF Core or another LINQ provider can translate the mapping expression into a query.

```csharp
var customers = await db.Customers
    .ProjectAs<CustomerDto>(mapperConfig)
    .ToListAsync();
```

`ProjectAs` is designed to stay SQL-friendly. Avoid using normal runtime-only C# methods inside `ProjectModel` expressions.

## Nested collections

For collection bridges such as `List<OrderItem>` to `List<OrderLineDto>`, use `MapField` on the parent collection and define a separate child map.

```csharp
cfg.MapModel<Order, OrderDetailDto>()
    .MapField(d => d.Lines, s => s.Items);

cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity);
```

`MapFieldStrict` requires source and destination member types to match, so it should not be used directly for a collection bridge where the item types are different.

## Benchmark notes

Public benchmark project:

```text
https://github.com/zameer-hussain/ZawkMapper.Benchmarks
```

The benchmark compares ZawkMapper, AutoMapper, and Manual Mapping across runtime DTO mapping, nested object mapping, collection mapping, EF Core projection, time, memory, per-record cost, and checksum validation.

Results vary by hardware, .NET runtime, database provider, data shape, and background workload.

## Honest performance position

Manual mapping remains the best baseline for raw hand-written runtime speed.

ZawkMapper 0.6.1 is competitive in flat DTO and summary DTO runtime mapping scenarios and strong in EF Core `ProjectAs` projection scenarios.

Nested runtime collection mapping remains one of the next optimization targets.

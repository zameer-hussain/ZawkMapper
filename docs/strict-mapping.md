# Strict mapping

`MapFieldStrict` is for same-type member mapping with compile-time safety.

Use it when the destination member type and source expression type match.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapFieldStrict(d => d.Id, s => s.Id)
    .MapFieldStrict(d => d.Name, s => s.Name)
    .MapFieldStrict(d => d.Price, s => s.Price);
```

Best use cases:

- IDs
- names
- dates
- numeric values with the same type
- booleans
- enum fields with the same enum type

Do not use `MapFieldStrict` for type conversion.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.PriceText, s => s.Price);
```

Do not use it for parent collection bridges when item types differ.

```csharp
cfg.MapModel<Order, OrderDetailDto>()
    .MapField(d => d.Lines, s => s.Items);
```

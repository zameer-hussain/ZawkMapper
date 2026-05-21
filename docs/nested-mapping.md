# Nested mapping

ZawkMapper supports nested object and collection mapping when a child map is registered.

## Nested object

```csharp
cfg.MapModel<Customer, CustomerDto>()
    .MapField(d => d.Address, s => s.Address);

cfg.MapModel<Address, AddressDto>()
    .MapFieldStrict(d => d.City, s => s.City)
    .MapFieldStrict(d => d.Country, s => s.Country);
```

## Nested collection

Use `MapField` for the parent collection bridge, then use strict/direct mapping inside the child item map where possible.

```csharp
cfg.MapModel<Order, OrderDetailDto>()
    .MapField(d => d.Lines, s => s.Items);

cfg.MapModel<OrderItem, OrderLineDto>()
    .MapFieldStrict(d => d.ProductName, s => s.ProductName)
    .MapFieldStrict(d => d.Quantity, s => s.Quantity)
    .MapFieldStrict(d => d.UnitPrice, s => s.UnitPrice);
```

`List<OrderItem>` and `List<OrderLineDto>` are different member types. That is why the parent collection bridge uses `MapField`.

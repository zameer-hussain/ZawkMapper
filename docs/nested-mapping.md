# Nested Mapping

ZawkMapper can reuse configured child projections in parent projections.

## Child projection

```csharp
cfg.ProjectModel<Order, OrderListDto>()
   .MapFieldStrict(dest => dest.OrderId, src => src.Id)
   .MapFieldStrict(dest => dest.OrderNumber, src => src.OrderNumber);
```

## Parent projection with child collection

```csharp
cfg.ProjectModel<Customer, CustomerDetailsDto>()
   .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
   .MapField(dest => dest.Orders, src => src.Orders
       .Where(o => !o.IsDeleted)
       .OrderByDescending(o => o.OrderDateUtc)
       .Take(10));
```

## Parent projection with single child

```csharp
cfg.ProjectModel<Customer, CustomerDashboardDto>()
   .MapField(dest => dest.LastOrder, src => src.Orders
       .Where(o => !o.IsDeleted)
       .OrderByDescending(o => o.OrderDateUtc)
       .FirstOrDefault());
```

## Named child projection

Use named child projections only when needed.

```csharp
.MapField(dest => dest.Orders,
    src => src.Orders.Where(o => !o.IsDeleted),
    opt => opt.UseProjection("Small"));
```

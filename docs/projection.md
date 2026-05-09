# Projection Guide

`ProjectModel` and `ProjectAs` are designed for database query scenarios.

## Example

```csharp
cfg.ProjectModel<Customer, CustomerListDto>()
   .MapFieldStrict(dest => dest.CustomerId, src => src.Id)
   .MapFieldStrict(dest => dest.DisplayName, src => src.FullName)
   .MapField(dest => dest.OrdersCount, src => src.Orders.Count(o => !o.IsDeleted));

var result = db.Customers
    .Where(x => !x.IsDeleted)
    .ProjectAs<CustomerListDto>(config)
    .ToList();
```

## SQL-friendly rule

Projection should generate provider-friendly expression trees. Avoid runtime-only logic inside `ProjectAs`.

Good:

```csharp
.MapField(dest => dest.StatusText, src =>
    src.Status == CustomerStatus.Active ? "Active" : "Unknown")
```

Avoid relying on runtime conversion in database projection.

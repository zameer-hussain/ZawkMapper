# Projection

Projection is for `IQueryable` queries. It lets the database provider select DTO fields instead of loading full entities first.

```csharp
var customers = await db.Customers
    .ProjectAs<CustomerListDto>(_mapperConfig)
    .ToListAsync();
```

## Register projection rules

```csharp
cfg.ProjectModel<Customer, CustomerListDto>()
    .MapField(d => d.Id, s => s.Id)
    .MapField(d => d.Name, s => s.Name)
    .MapField(d => d.OrdersCount, s => s.Orders.Count());
```

## Runtime map and projection can be different

Runtime mapping can use normal C# code. Projection should use expressions that the query provider can translate.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => FormatProductName(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

`ProjectAs` uses `ProjectModel` first. If no `ProjectModel` exists, it may fall back to `MapModel` when the mapping is projection-safe.

## Named projections

Named projections are useful for list/detail/edit screens, public/admin views, and language-based fields.

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

ZawkMapper does not include built-in language enums. Names are developer-owned strings, so constants are recommended.

## EF Core global query filters

If EF Core already has a global query filter, avoid repeating the same filter inside the projection unless you need it on purpose.

```csharp
modelBuilder.Entity<Order>()
    .HasQueryFilter(x => !x.IsDeleted);
```

Use:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count())
```

Instead of repeating:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count(x => !x.IsDeleted))
```

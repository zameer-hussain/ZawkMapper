# Projection

Projection is used when you want an `IQueryable` to return DTOs directly.

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig)
    .ToListAsync();
```

You can also pass a cached configuration method:

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(AppMappingConfig.StaticConfigMethod())
    .ToListAsync();
```

Both are fine when the method returns the same cached configuration object.

## Register projection rules

```csharp
cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Id, s => s.Id)
    .MapField(d => d.Name, s => s.NameEn);
```

`TSource` is the source type. Usually this is your database entity.

`TDestination` is the destination type. Usually this is your DTO.

## ProjectModel and MapModel

`ProjectModel` is checked first by `ProjectAs`.

If no `ProjectModel` exists, `ProjectAs` can reuse `MapModel` when the rules are projection-safe.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

This can still work:

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig)
    .ToListAsync();
```

When runtime and projection rules are different, register both:

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => SomeNormalCSharpMethod(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

Runtime `Map` uses `MapModel`.

`ProjectAs` uses `ProjectModel`.

## Named projections

Use names for list/detail/language/scenario choices.

```csharp
public static class ProductProjectionNames
{
    public const string English = "English";
    public const string Sindhi = "Sindhi";
}//ProductProjectionNames
```

```csharp
cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.Sindhi)
    .MapField(d => d.Name, s => s.NameSd);
```

```csharp
var projectionName = isSindhi
    ? ProductProjectionNames.Sindhi
    : ProductProjectionNames.English;

var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig, projectionName)
    .ToListAsync();
```

## EF Core global filters

If your entity already has an EF Core global query filter, do not repeat the same filter inside `ProjectModel` unless you intentionally want it in the expression.

For example, if `Order` already has `HasQueryFilter(x => !x.IsDeleted)`, this is enough:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count())
```

Avoid repeating the same filter like this in that case:

```csharp
.MapField(d => d.OrdersCount, s => s.Orders.Count(o => !o.IsDeleted))
```

ZawkMapper keeps your expression as you wrote it. EF Core applies its own global filters later while building SQL.

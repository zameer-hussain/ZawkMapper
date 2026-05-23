# Runtime mapping and projection

ZawkMapper 0.6.1 keeps runtime mapping and query projection as separate registrations.

`MapModel` is used by runtime `Map`.

`ProjectModel` is used by `ProjectAs`.

This change is useful in real projects because runtime mapping can use normal C# code, while database projection should stay friendly for EF Core and SQL providers.

## Same source and DTO can have both rules

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => SomeNormalCSharpMethod(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

Runtime use:

```csharp
var dto = _mapper.Map<Product, ProductDto>(product);
```

Projection use:

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig)
    .ToListAsync();
```

You can also pass the cached configuration directly:

```csharp
var products = await db.Products
    .ProjectAs<ProductDto>(AppMappingConfig.StaticConfigMethod())
    .ToListAsync();
```

Both calls use the same cached `MapperConfiguration` object when `StaticConfigMethod()` returns `Cached.Value`.

## Duplicate rules

Two unnamed `MapModel<Product, ProductDto>()` registrations are not allowed.

Two unnamed `ProjectModel<Product, ProductDto>()` registrations are not allowed.

One `MapModel<Product, ProductDto>()` and one `ProjectModel<Product, ProductDto>()` are allowed.

## Fallback behavior

If `ProjectModel` exists, `ProjectAs` uses it.

If `ProjectModel` does not exist, `ProjectAs` can reuse `MapModel` when the map is projection-safe.

This keeps simple apps easy, but still lets real apps use different runtime and SQL projection rules.

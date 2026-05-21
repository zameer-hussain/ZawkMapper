# Runtime mapping vs projection

ZawkMapper supports two related but different jobs.

| Job | API | When to use |
|---|---|---|
| Runtime mapping | `MapModel` | object is already loaded in memory |
| Query projection | `ProjectModel` + `ProjectAs` | data comes from `IQueryable` / EF Core |

## Runtime mapping

Runtime mapping can use normal C# logic because it runs in memory.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => FormatName(s.NameEn));
```

## Projection

Projection should stay SQL-friendly.

```csharp
cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

## Same source and destination

You may register both for the same source and destination.

```csharp
cfg.MapModel<Product, ProductDto>()
    .MapField(d => d.Name, s => FormatName(s.NameEn));

cfg.ProjectModel<Product, ProductDto>()
    .MapField(d => d.Name, s => s.NameEn);
```

This is allowed because runtime mapping and projection can have different requirements.

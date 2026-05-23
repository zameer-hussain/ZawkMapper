# Changelog

## 0.6.1

### Stable release

ZawkMapper 0.6.1 promotes the tested 0.6.1 release candidate work to stable after public NuGet testing and benchmark validation.

This release focuses on runtime object mapping performance, allocation reduction, stable EF Core projection behavior, clearer documentation, and stronger package metadata for discoverability as a .NET object mapper, C# DTO mapping library, AutoMapper alternative, and EF Core IQueryable projection tool.

### Changed

- Runtime `MapModel` execution uses a cached assignment plan for common mapping paths.
- Directly assignable fields avoid unnecessary runtime conversion.
- `MapField`, `MapFieldDirect`, and `MapFieldStrict` benefit from faster same-type member assignment paths where applicable.
- `MapList` reuses one mapping context and preallocates result lists when source count is known.
- Runtime collection conversion caches generic collection converters and preallocates destination lists when possible.
- Runtime path tracking avoids unnecessary allocation for normal one-level and two-level mapping paths.
- `MappingContext` creates the public `Items` dictionary only when it is used.
- Internal map lookup keys use a readonly struct to reduce repeated allocation in hot paths.
- Public docs now explain when to use `MapField`, `MapFieldDirect`, and `MapFieldStrict`.
- Package metadata now includes clearer SEO-friendly wording for ZawkMapper, ZawkTech, .NET object mapper, C# DTO mapping, AutoMapper alternative, EF Core projection, and IQueryable projection scenarios.

### Guidance

Use `MapFieldStrict` for same-type member mapping when compile-time safety is important.

Use `MapFieldDirect` for direct same-type assignment when you want explicit direct mapping behavior.

Use `MapField` for flexible conversion, computed fields, nested object mapping, and collection bridges.

For collection bridges such as `List<OrderItem>` to `List<OrderLineDto>`, use `MapField` on the parent collection and define a child map for `OrderItem` to `OrderLineDto`.

Projection behavior is intentionally preserved. `ProjectAs` remains SQL-friendly and should be compared with manual `Select` and AutoMapper `ProjectTo` in real application scenarios.

### Validation

Final benchmark testing against the published NuGet package confirmed that ZawkMapper 0.6.1 keeps the expected release behavior:

- strong EF Core `ProjectAs` projection performance
- competitive flat DTO runtime mapping
- competitive summary DTO runtime mapping
- lower allocation than AutoMapper in some flat and summary runtime benchmark scenarios
- nested runtime collection mapping remains a future optimization target

Benchmark repository:

```text
https://github.com/zameer-hussain/ZawkMapper.Benchmarks
```

## 0.6.1-rc.1

### Release candidate

This release candidate focused on runtime object mapping performance, allocation reduction, clearer documentation, and package metadata improvements while keeping the public ZawkMapper API unchanged.

### Changed

- Runtime `MapModel` execution moved toward cached assignment planning for common mapping paths.
- Directly assignable fields avoided runtime conversion when the source expression type could be assigned to the destination member type.
- Runtime list and collection mapping reduced allocation in common paths.
- Runtime path tracking became cheaper for common mapping paths.
- Package metadata and public docs were improved for .NET object mapper, C# DTO mapping, AutoMapper alternative, EF Core projection, and IQueryable projection scenarios.

## 0.6.0

### Stable release

This was the first stable 0.6.0 release after the 0.6.0 release candidate feedback period.

### Changed

- `MapModel` and `ProjectModel` are handled as separate registration types.
- Duplicate runtime mappings and duplicate projections still throw for the same source, destination, and name.
- One `MapModel` and one `ProjectModel` for the same source, destination, and name are allowed.
- `ProjectAs` uses `ProjectModel` first. If no projection exists, it can reuse `MapModel` when mapping rules are projection-safe.
- MVC sample projection maps avoid repeating `IsDeleted` checks where EF Core global query filters already apply them.

### Added

- Clearer XML documentation for public APIs.
- Clearer duplicate and missing configuration error messages.
- MVC sample scenarios for English and Sindhi named projections, runtime mapping versus projection, salary increment, prefix and suffix, and static cached configuration usage.

## 0.5.0-rc.4

Tested release candidate with runtime mapping, strict mapping, direct mapping, flexible conversion, nested projection, named projection, MVC sample, smoke tests, unit tests, and performance lab.

# Changelog

## 0.6.1-rc.1

### Release candidate

This release candidate focuses on runtime object mapping performance, lower allocation, clearer public documentation, package metadata quality, and stable `ProjectAs` projection behavior.

### Changed

- Runtime `MapModel` execution now uses cached runtime plans for repeated mapping calls.
- Same-type runtime members can avoid unnecessary conversion work.
- Simple runtime maps avoid creating extra path-tracking state when it is not needed.
- Runtime collection conversion preallocates destination lists when the source count is known.
- `MappingContext.Items` is lazy and does not allocate unless a developer actually uses it.
- Internal map lookup keys use a readonly struct to reduce hot-path allocation.
- Public docs now explain when to use `MapFieldStrict`, `MapFieldDirect`, and `MapField`.
- Docs are split into public docs and private owner/developer notes.
- README and package wording now include clearer .NET object mapper, C# DTO mapping, AutoMapper alternative, EF Core projection, and IQueryable projection wording.

### Guidance

Use `MapFieldStrict` for same-type member mapping when possible.

Use `MapField` for runtime conversion, computed values, nested object mapping, and collection bridges.

For a collection bridge such as `List<OrderItem>` to `List<OrderLineDto>`, use `MapField` on the parent collection member and define a separate child map for `OrderItem` to `OrderLineDto`.

`ProjectAs` projection behavior is intentionally preserved and should remain provider-friendly for EF Core `IQueryable` scenarios.

### Validation before publish

Run restore, release build, smoke tests, unit-style tests, performance lab, MVC sample, and the external comparison benchmark before publishing.

## 0.6.0

### Stable release

This was the first stable 0.6.0 release after the 0.6.0 release-candidate feedback period.

### Changed

- `MapModel` and `ProjectModel` are handled as separate registration types.
- Duplicate `MapModel` registrations for the same source, destination, and name still throw.
- Duplicate `ProjectModel` registrations for the same source, destination, and name still throw.
- One `MapModel` and one `ProjectModel` for the same source, destination, and name are allowed.
- `ProjectAs` uses `ProjectModel` first.
- If no projection exists, `ProjectAs` can reuse `MapModel` when the mapping rules are projection-safe.
- MVC sample projection maps avoid repeating `IsDeleted` checks where EF Core global query filters already apply them.

### Added

- Clearer XML documentation for public APIs.
- Clearer duplicate and missing configuration error messages.
- MVC sample scenarios for English and Sindhi named projections, runtime mapping versus projection, salary increment, prefix and suffix, and static cached configuration usage.

### Guidance

Use constants or static readonly values for map and projection names to avoid spelling mistakes.

Use cached app-level configuration for reusable rules.

Use named projections for finite choices such as English, Sindhi, list, detail, public, or admin.

Use scenario-level configuration when the mapping expression depends on request values such as salary increment percentage, prefix, or suffix.

If an entity already has an EF Core global query filter, avoid writing the same filter again inside `ProjectModel` unless you intentionally want that extra condition.

## 0.5.0-rc.4

Tested release candidate with runtime mapping, strict mapping, direct mapping, flexible conversion, nested projection, named projection, MVC sample, smoke tests, unit-style tests, and performance lab.

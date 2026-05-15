# Changelog

## 0.6.0

### Stable release

This is the first stable 0.6.0 release after the 0.6.0 release candidate feedback period.

### Changed

`MapModel` and `ProjectModel` are handled as separate registration types.

A duplicate `MapModel` for the same source, destination, and name still throws.

A duplicate `ProjectModel` for the same source, destination, and name still throws.

One `MapModel` and one `ProjectModel` for the same source, destination, and name are allowed.

`ProjectAs` uses `ProjectModel` first. If no projection exists, it can reuse `MapModel` when the mapping rules are projection-safe.

MVC sample projection maps avoid repeating `IsDeleted` checks where EF Core global query filters already apply them.

### Added

Clearer XML documentation for public APIs.

Clearer duplicate and missing configuration error messages.

MVC sample scenarios for English and Sindhi named projections, runtime mapping versus projection, salary increment, prefix and suffix, and static cached configuration usage.

### Guidance

Use constants or static readonly values for map/projection names to avoid spelling mistakes.

Use cached app-level configuration for reusable rules.

Use named projections for finite choices such as English, Sindhi, list, detail, public, or admin.

Use scenario-level configuration when the mapping expression depends on request values such as salary increment percentage, prefix, or suffix.

If an entity already has an EF Core global query filter, avoid writing the same filter again inside `ProjectModel` unless you intentionally want that extra condition.


## 0.6.0-rc.2

### Changed

MVC sample projection maps now avoid repeating `IsDeleted` checks where EF Core global query filters already apply them.

Runtime cycle detection uses a cached path key on each map definition, so repeated runtime mapping avoids building that key again and again.

README and docs now include updated project credits.

### Guidance

If an entity already has an EF Core global query filter, avoid writing the same filter again inside `ProjectModel` unless you intentionally want that extra condition in the expression.

Use constants or static readonly values for map/projection names to avoid spelling mistakes.

Use cached app-level configuration for reusable rules, named projections for finite choices, and scenario-level configuration when the mapping expression depends on request values.

## 0.6.0-rc.1

### Changed

`MapModel` and `ProjectModel` are now separate registration types.

A duplicate `MapModel` for the same source, destination, and name still throws.

A duplicate `ProjectModel` for the same source, destination, and name still throws.

One `MapModel` and one `ProjectModel` for the same source, destination, and name are now allowed.

`ProjectAs` uses `ProjectModel` first. If no projection exists, it can reuse `MapModel` when the mapping rules are projection-safe.

### Added

Clearer XML documentation for public APIs so method hover text explains source type, destination type, and usage.

Clearer duplicate and missing configuration error messages.

MVC sample pages for English and Sindhi named projection, runtime map versus projection, salary increment, prefix and suffix, and static cached configuration call.

Unit tests for separate runtime and projection registration, projection fallback, Sindhi named projection, and scenario-level request values.

Performance lab checks for runtime plus projection on the same pair, named Sindhi projection, and scenario-level salary increment projection.

### Guidance

Use constants or static readonly values for map/projection names to avoid spelling mistakes.

Keep package behavior generic. Do not add built-in language enums to the package.

Use cached app-level configuration for reusable rules.

Use named projections for finite choices such as English, Sindhi, list, detail, public, or admin.

Use scenario-level configuration when the mapping expression itself depends on request values such as salary increment percentage, prefix, or suffix.

## 0.5.0-rc.4

Tested release candidate with runtime mapping, strict mapping, direct mapping, flexible conversion, nested projection, named projection, MVC sample, smoke tests, unit tests, and performance lab.

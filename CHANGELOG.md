# Changelog

## 0.5.0-rc.2

- Added expanded performance lab scenarios for strict-only, flexible-only, and mixed mapping.
- Added cache hit/miss measurements for runtime plans and projection expressions.
- Added memory delta output to performance CSV.
- Added named projection, nested projection, collection-to-single projection, and conversion-specific performance scenarios.
- Added small map-resolution cache to reduce repeated named/default map lookup overhead.
- Added performance-lab documentation.

## 0.5.0-rc.1

- Added `MapFieldStrict` for compile-time same-type member mapping.
- Added `ForMemberStrict` compatibility alias.
- Kept `MapField` flexible for runtime conversion scenarios.
- Improved README and SEO-friendly documentation.
- Added strict mapping documentation.
- Added unit-style test project for core mapping scenarios.
- Added NuGet package icon support through `PackageIcon`.
- Updated package metadata for ZawkTech / Zameer Hussain Vighio.
- Kept package fully free with no license checks and no telemetry.

## 0.4.4-preview

- Fixed MVC sample namespace/reference issues.
- Cleaned sample mapping profile.

## 0.4.3-preview

- Added cached runtime mapping plan.
- Improved runtime mapping performance significantly.
- Added internal performance lab enhancements.

## 0.4.0-preview

- Renamed VighioMapper to ZawkMapper.
- Added official API names and compatibility aliases.
- Added nested projection reuse, named maps, profile scanning, and DI registration.

## 0.5.0-rc.4

- Added `MapFieldDirect` for direct assignment without flexible conversion.
- Added `ForMemberDirect` compatibility alias.
- Added unit tests for direct mapping success and direct mismatch failure.
- Expanded performance lab with direct-only scenarios.
- Added memory-leak loop checks with forced GC after repeated 100k-object mappings.
- Added direct mapping documentation.

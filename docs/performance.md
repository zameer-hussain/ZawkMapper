# Performance Guide

ZawkMapper is designed to reuse configuration and mapping plans.

## Recommended production setup

- create configuration once
- use static cached configuration or dependency injection singleton
- use `ProjectAs` for database-backed screens
- avoid creating mapping configuration repeatedly

## Static cached example

```csharp
public static class AppMappingConfig
{
    private static readonly Lazy<MapperConfiguration> Cached = new(() =>
        new MapperConfiguration(cfg =>
        {
            cfg.AddProfilesFromAssembly(typeof(AppMappingConfig).Assembly);
        }));

    public static MapperConfiguration StaticConfigMethod()
    {
        return Cached.Value;
    }//StaticConfigMethod
}//AppMappingConfig
```

## What to measure

- configuration build time
- first projection build time
- cached projection reuse time
- runtime mapping cold call
- runtime mapping warm call
- runtime mapping 10k and 100k objects
- memory before/after repeated operations

## rc.2 performance lab scenarios

The internal performance lab now separates strict, flexible, and mixed mapping scenarios.

### Mapping styles measured

- `MapFieldStrict` only
- flexible `MapField` only
- mixed strict + flexible mapping
- named projections
- nested projections
- cache hit scenarios
- static cached configuration
- singleton-like configuration
- new configuration per call

### Important scenarios

```text
config_build_strict_only
config_build_flexible_only
config_build_mixed
config_build_projection_nested_named
manual_map_100k_baseline
strict_single_object_cold
strict_single_object_warm
flexible_single_object_cold
flexible_single_object_warm
mixed_single_object_cold
mixed_single_object_warm
runtime_map_100k_objects_strict_warm
runtime_map_100k_objects_flexible_warm
runtime_map_100k_objects_mixed_warm
conversion_string_to_long_100k
conversion_string_to_decimal_100k
conversion_string_to_bool_100k
conversion_int_to_enum_100k
conversion_enum_to_string_100k
project_simple_cached_reuse
project_named_edit_cached_reuse
project_named_detail_cached_reuse
nested_single_child_10k
nested_child_collection_10k
collection_firstordefault_projection_10k
cache_runtime_plan_hit_10k_calls
cache_projection_expression_hit_10k_calls
static_cached_config_runtime_100k
singleton_like_config_runtime_100k
new_config_each_call_runtime_1k
```

### CSV columns

```text
scenario,map_style,mode,records,elapsed_ms,memory_before_bytes,memory_after_bytes,memory_delta_bytes,notes
```

### How to read results

- strict mapping should usually be fastest after warmup.
- flexible mapping may be slower when conversions are involved.
- mixed mapping should usually sit between strict and flexible.
- static cached and singleton-like config should be close.
- new config per call is supported, but not recommended for production web apps.


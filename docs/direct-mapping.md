# Direct Mapping

`MapFieldDirect` maps a member by direct assignment without flexible runtime conversion.

Use it when the source value is already assignable to the destination member type and you want a clear runtime error if the types are not assignable.

```csharp
cfg.MapModel<User, UserDto>()
   .MapFieldDirect(dest => dest.Id, src => src.Id)
   .MapFieldDirect(dest => dest.Email, src => src.Email);
```

## Recommendation

For same-type fields, prefer `MapFieldStrict` because it gives compile-time same-type safety and is usually the best runtime mapping choice.

Use `MapFieldDirect` when you intentionally want direct-assignment behavior but the compile-time generic shape is not as strict as `MapFieldStrict`.

Use `MapField` when conversion is intentional.

## Behavior

`MapFieldDirect` does not try to convert values.

If the source value cannot be assigned to the destination member, ZawkMapper throws a clear `MappingException`.

```csharp
cfg.MapModel<UserTextId, UserDto>()
   .MapFieldDirect(dest => dest.Id, src => src.Id);
```

If `UserTextId.Id` is `string` and `UserDto.Id` is `long`, this will fail at runtime with a clear message.

Use this instead when conversion is intentional:

```csharp
cfg.MapModel<UserTextId, UserDto>()
   .MapField(dest => dest.Id, src => src.Id);
```

## Strict vs Direct vs Flexible

| Method | Meaning | Best use |
|---|---|---|
| `MapFieldStrict` | compile-time same-type safety | best default for same-type fields |
| `MapFieldDirect` | direct assignment, no conversion | assignable fields with runtime validation |
| `MapField` | flexible runtime conversion | computed, converted, nested, or collection bridge fields |

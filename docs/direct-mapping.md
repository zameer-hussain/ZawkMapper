# Direct Mapping

`MapFieldDirect` is for developers who want direct assignment without flexible conversion.

Use it when the source value is already assignable to the destination member type and you want ZawkMapper to avoid conversion work.

```csharp
cfg.MapModel<User, UserDto>()
   .MapFieldDirect(dest => dest.Id, src => src.Id)
   .MapFieldDirect(dest => dest.Email, src => src.Email);
```

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

| Method | Meaning |
|---|---|
| `MapFieldStrict` | compile-time same-type safety |
| `MapFieldDirect` | direct assignment, no conversion |
| `MapField` | flexible runtime conversion |

## Recommended usage

Use `MapFieldStrict` when you want maximum compile-time safety.

Use `MapFieldDirect` when types are already compatible and you want direct assignment behavior.

Use `MapField` when you intentionally want ZawkMapper to help with conversion, such as string to long, enum to string, or int to enum.

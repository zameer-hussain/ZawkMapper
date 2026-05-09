# Strict Mapping

ZawkMapper supports both flexible mapping and compile-time strict mapping.

## When to use MapFieldStrict

Use `MapFieldStrict` when source and destination values must have the same type.

```csharp
cfg.MapModel<User, UserDto>()
   .MapFieldStrict(dest => dest.Id, src => src.Id)
   .MapFieldStrict(dest => dest.Email, src => src.Email);
```

This is useful for sensitive or important fields where silent conversion is not desired.

## Compile-time safety

`MapFieldStrict` is intentionally designed with one generic member type. That means both expressions must match the same type.

This should compile:

```csharp
.MapFieldStrict(dest => dest.Email, src => src.Email)
```

This should not compile if `dest.Id` is `long` and `src.Id` is `string`:

```csharp
.MapFieldStrict(dest => dest.Id, src => src.Id)
```

## When to use MapField

Use `MapField` when flexible mapping is wanted.

```csharp
cfg.MapModel<UserCreateDto, User>()
   .MapField(dest => dest.Id, src => src.Id);
```

Runtime mapping can try safe conversion if enabled.

## Enum handling

Runtime mapping can convert common enum scenarios:

- enum to string
- enum to numeric value
- numeric value to enum
- string name to enum where possible

For database projection, prefer explicit SQL-friendly expressions.

```csharp
cfg.ProjectModel<User, UserDto>()
   .MapField(dest => dest.StatusText, src =>
       src.Status == UserStatus.Active ? "Active" : "Unknown");
```

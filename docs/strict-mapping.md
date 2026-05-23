# Strict Mapping

`MapFieldStrict` is the recommended mapping method for same-type runtime fields.

It gives compile-time same-type safety and uses ZawkMapper's fastest runtime assignment path for normal DTO mapping.

```csharp
cfg.MapModel<User, UserDto>()
   .MapFieldStrict(dest => dest.Id, src => src.Id)
   .MapFieldStrict(dest => dest.Email, src => src.Email)
   .MapFieldStrict(dest => dest.CreatedAtUtc, src => src.CreatedAtUtc);
```

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

Use `MapField` when flexible mapping is wanted.

```csharp
cfg.MapModel<UserCreateDto, User>()
   .MapField(dest => dest.Id, src => src.Id);
```

Runtime mapping can try safe conversion if enabled.

## Nested collection bridge note

`MapFieldStrict` checks that the source expression and destination member use the same member type. Because of that, it is not the right method for a parent collection bridge when the element DTO type changes.

This is not a strict same-type member:

```csharp
// List<OrderItem> is not the same type as List<OrderLineDto>.
.MapFieldStrict(dest => dest.Lines, src => src.Items)
```

Use `MapField` on the parent collection member, then configure the child map separately with `MapFieldStrict` where the child fields are same-type.

```csharp
cfg.MapModel<Order, OrderDetailDto>()
   .MapField(dest => dest.Lines, src => src.Items);

cfg.MapModel<OrderItem, OrderLineDto>()
   .MapFieldStrict(dest => dest.ProductName, src => src.ProductName)
   .MapFieldStrict(dest => dest.Quantity, src => src.Quantity)
   .MapFieldStrict(dest => dest.UnitPrice, src => src.UnitPrice);
```

This pattern is useful for nested object mapping, collection mapping, DTO mapping, and AutoMapper alternative comparison benchmarks because it keeps the parent bridge flexible while keeping the child map strongly typed.

## Projection note

For database projection, prefer SQL-friendly expressions.

```csharp
cfg.ProjectModel<User, UserDto>()
   .MapField(dest => dest.StatusText, src =>
       src.Status == UserStatus.Active ? "Active" : "Unknown");
```

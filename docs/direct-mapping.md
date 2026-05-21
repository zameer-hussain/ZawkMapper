# Direct mapping

`MapFieldDirect` is for direct assignment when the source value is assignable to the destination member.

```csharp
cfg.MapModel<Customer, CustomerDto>()
    .MapFieldDirect(d => d.Id, s => s.Id)
    .MapFieldDirect(d => d.Name, s => s.Name);
```

Use `MapFieldStrict` when you want compile-time same-type safety.

Use `MapField` when conversion or computed values are needed.

```csharp
cfg.MapModel<Customer, CustomerDto>()
    .MapFieldStrict(d => d.Id, s => s.Id)
    .MapField(d => d.BalanceText, s => s.Balance);
```

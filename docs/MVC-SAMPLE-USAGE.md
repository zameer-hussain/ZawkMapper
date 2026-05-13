# MVC sample usage

Run the sample:

```bash
dotnet run --project samples/ZawkMapper.MvcCrudSample/ZawkMapper.MvcCrudSample.csproj
```

Open the browser URL shown by dotnet.

The sample includes normal CRUD and these ZawkMapper pages:

1. English and Sindhi named projection
2. Runtime MapModel and SQL-friendly ProjectModel for the same source and DTO
3. Salary increment with request-level configuration
4. Prefix and suffix with request-level configuration
5. Static cached configuration call using `AppMappingConfig.StaticConfigMethod()`

The sample uses `AddZawkMapper` in `Program.cs`.

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(ProductMappingProfile).Assembly);
});
```

This already registers `IObjectMapper`. Do not register it again unless you are manually creating the configuration.


## Global filter note

The sample uses EF Core global query filters for soft delete. Because of that, the customer projection does not repeat `o => !o.IsDeleted` inside `ProjectModel`. This keeps the generated SQL clean and avoids duplicate filter conditions.

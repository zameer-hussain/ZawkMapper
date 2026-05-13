# Dependency injection

For ASP.NET Core MVC or API projects, use `AddZawkMapper` in `Program.cs`.

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(ProductMappingProfile).Assembly);
});
```

This registers `MapperConfiguration` as singleton and `IObjectMapper` as scoped.

You do not need to manually add this when `AddZawkMapper` is already used:

```csharp
builder.Services.AddScoped<IObjectMapper, ObjectMapper>();
```

Manual registration is only needed when you intentionally create `MapperConfiguration` yourself.

```csharp
var mapperConfiguration = AppMappingConfig.StaticConfigMethod();
builder.Services.AddSingleton(mapperConfiguration);
builder.Services.AddScoped<IObjectMapper, ObjectMapper>();
```

Both styles are valid. The first style is cleaner for most MVC and API projects.

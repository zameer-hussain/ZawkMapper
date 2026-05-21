# Real-world scenarios

## Language-based projection

Use named projections when the options are known.

```csharp
public static class ProductProjectionNames
{
    public const string English = "English";
    public const string Sindhi = "Sindhi";
}//ProductProjectionNames
```

```csharp
cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.English)
    .MapField(d => d.Name, s => s.NameEn);

cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.Sindhi)
    .MapField(d => d.Name, s => s.NameSd);
```

```csharp
var projectionName = isSindhi
    ? ProductProjectionNames.Sindhi
    : ProductProjectionNames.English;
```

## Request value in projection

Scenario-level configuration is useful when a value comes from the request.

```csharp
public static MapperConfiguration CreateEmployeeProjectionConfig(decimal incrementPercent)
{
    return new MapperConfiguration(cfg =>
    {
        cfg.ProjectModel<Employee, EmployeeDto>()
            .MapField(d => d.Id, s => s.Id)
            .MapField(d => d.FullName, s => s.FullName)
            .MapField(d => d.ProjectedSalary, s => s.Salary + (s.Salary * incrementPercent / 100));
    });
}//CreateEmployeeProjectionConfig
```

Use cached app-level configuration for reusable rules. Use scenario-level configuration when the expression itself changes per request.

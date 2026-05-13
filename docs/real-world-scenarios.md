# Real world scenarios

These examples show the patterns used in the MVC sample.

## Named language projection

Use constants for names. This avoids spelling mistakes when the same name is used in many files.

```csharp
public static class ProductProjectionNames
{
    public const string English = "English";
    public const string Sindhi = "Sindhi";
}//ProductProjectionNames
```

Register named projections:

```csharp
cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.English)
    .MapField(d => d.Name, s => s.NameEn)
    .MapField(d => d.Description, s => s.DescriptionEn);

cfg.ProjectModel<Product, ProductDto>(ProductProjectionNames.Sindhi)
    .MapField(d => d.Name, s => s.NameSd)
    .MapField(d => d.Description, s => s.DescriptionSd);
```

Use `isSindhi` at request time:

```csharp
var projectionName = isSindhi
    ? ProductProjectionNames.Sindhi
    : ProductProjectionNames.English;

var products = await db.Products
    .ProjectAs<ProductDto>(_mapperConfig, projectionName)
    .ToListAsync();
```

This keeps Sindhi, Sindh Pakistan culture visible in the sample without adding language-specific enums inside the package.

## Request value inside projection

Sometimes a value comes from the current request. Examples are salary increment percentage, a prefix, a suffix, a branch code, or tenant-specific display text.

For these cases, scenario-level configuration is valid.

```csharp
public static MapperConfiguration CreateSalaryProjectionConfig(decimal incrementPercent)
{
    return new MapperConfiguration(cfg =>
    {
        cfg.ProjectModel<Employee, EmployeeSalaryDto>()
            .MapField(d => d.Id, s => s.Id)
            .MapField(d => d.ProjectedSalary, s => s.Salary + (s.Salary * incrementPercent / 100));
    });
}//CreateSalaryProjectionConfig
```

Use it:

```csharp
var config = EmployeeScenarioConfig.CreateSalaryProjectionConfig(request.IncrementPercent);

var employees = await db.Employees
    .ProjectAs<EmployeeSalaryDto>(config)
    .ToListAsync();
```

This is flexible. It is also slower than reusing one cached app-level configuration, so use it where the scenario really needs runtime values.

## Prefix and suffix

```csharp
public static MapperConfiguration CreatePrefixSuffixProjectionConfig(string prefix, string suffix)
{
    return new MapperConfiguration(cfg =>
    {
        cfg.ProjectModel<Product, ProductPrefixSuffixDto>()
            .MapField(d => d.Id, s => s.Id)
            .MapField(d => d.DisplayName, s => prefix + s.NameEn + suffix);
    });
}//CreatePrefixSuffixProjectionConfig
```

This keeps ZawkMapper useful for dynamic projection cases while still making cached configuration the best normal choice.

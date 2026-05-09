# MVC CRUD Sample Usage

This sample is added so you can test ZawkMapper in a real ASP.NET Core MVC project with EF Core and database records.

## What this sample contains

- ASP.NET Core MVC project
- EF Core SQLite database
- 2 DB tables: Customers and Orders
- 1200 seeded customers
- 2400 seeded orders
- Runtime mapping: `CustomerCreateDto -> Customer`
- Runtime mapping: `CustomerEditDto -> Customer`
- Projection mapping: `Customer -> CustomerListDto`
- Projection mapping: `Customer -> CustomerDetailsDto`
- CRUD flow
- SQL-based UPDATE
- SQL-based soft DELETE

## Run

```bash
dotnet restore samples/ZawkMapper.MvcCrudSample/ZawkMapper.MvcCrudSample.csproj
dotnet run --project samples/ZawkMapper.MvcCrudSample/ZawkMapper.MvcCrudSample.csproj
```

Open the shown localhost URL.

## Static configuration style

The mapping configuration is in:

```text
samples/ZawkMapper.MvcCrudSample/Mapping/AppMappingConfig.cs
```

Usage style:

```csharp
public static MapperConfiguration StaticConfigMethod() => CreateConfiguration();
```

Then:

```csharp
var projected = db.Customers
    .ProjectTo<CustomerListDto>(AppMappingConfig.StaticConfigMethod())
    .ToList();
```

In real projects, create the config once in DI and reuse it instead of creating it per query.

## Runtime map usage

```csharp
var entity = mapper.Map<CustomerCreateDto, Customer>(dto)!;
```

## Projection usage

```csharp
var list = await db.Customers
    .ProjectTo<CustomerListDto>(mapperConfiguration)
    .ToListAsync();
```

## Important note about UPDATE/DELETE

This sample uses SQLite to avoid SQL Server setup. Therefore it uses `SqliteParameter`.

In your main SQL Server project, use the same pattern with:

```csharp
new SqlParameter("@Name", value)
```

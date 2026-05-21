# MVC sample usage

The MVC sample demonstrates:

- runtime object mapping
- SQL-friendly projection
- named English and Sindhi projections
- nested customer/order mapping
- salary increment scenario projection
- prefix/suffix scenario projection
- repository/service/controller separation
- static cached configuration usage

Run:

```bash
dotnet run --project samples/ZawkMapper.MvcCrudSample/ZawkMapper.MvcCrudSample.csproj -c Release
```

Check that list pages use projection where possible and runtime mapping where data is already loaded.

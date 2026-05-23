# Getting started

Install the package:

```bash
dotnet add package ZawkMapper --prerelease
```

Create a profile:

```csharp
public sealed class ProductMappingProfile : MappingProfile
{
    public override void Configure(MapperConfiguration cfg)
    {
        cfg.MapModel<ProductCreateDto, Product>()
            .MapField(d => d.NameEn, s => s.Name)
            .MapField(d => d.Price, s => s.Price);

        cfg.ProjectModel<Product, ProductListDto>()
            .MapField(d => d.Id, s => s.Id)
            .MapField(d => d.Name, s => s.NameEn)
            .MapField(d => d.Price, s => s.Price);
    }//Configure
}//ProductMappingProfile
```

Register ZawkMapper in `Program.cs`:

```csharp
builder.Services.AddZawkMapper(cfg =>
{
    cfg.AddProfilesFromAssembly(typeof(ProductMappingProfile).Assembly);
});
```

Inject runtime mapper and configuration where needed:

```csharp
public sealed class ProductService
{
    private readonly IObjectMapper _mapper;
    private readonly MapperConfiguration _mapperConfig;
    private readonly AppDbContext _db;

    public ProductService(
        IObjectMapper mapper,
        MapperConfiguration mapperConfig,
        AppDbContext db)
    {
        _mapper = mapper;
        _mapperConfig = mapperConfig;
        _db = db;
    }//ProductService

    public Product MapCreateDto(ProductCreateDto dto)
    {
        return _mapper.Map<ProductCreateDto, Product>(dto)!;
    }//MapCreateDto

    public Task<List<ProductListDto>> GetListAsync()
    {
        return _db.Products
            .ProjectAs<ProductListDto>(_mapperConfig)
            .ToListAsync();
    }//GetListAsync
}//ProductService
```

You can also pass a cached configuration method directly:

```csharp
var products = await db.Products
    .ProjectAs<ProductListDto>(AppMappingConfig.StaticConfigMethod())
    .ToListAsync();
```

Use this pattern when `StaticConfigMethod()` simply returns the same cached `MapperConfiguration` object.

## Credits

Developer and founder: Zameer Hussain Vighio.

Co-developer and contributor: Mr Aqib Ali Abbasi.

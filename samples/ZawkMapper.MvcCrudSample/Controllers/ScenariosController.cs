using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZawkMapper.Abstractions;
using ZawkMapper.Configuration;
using ZawkMapper.Extensions;
using ZawkMapper.MvcCrudSample.Data;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Mapping;
using ZawkMapper.MvcCrudSample.Models;

namespace ZawkMapper.MvcCrudSample.Controllers;

public sealed class ScenariosController : Controller
{
    private readonly AppDbContext _db;
    private readonly MapperConfiguration _mapperConfig;
    private readonly IObjectMapper _mapper;

    public ScenariosController(
        AppDbContext db,
        MapperConfiguration mapperConfig,
        IObjectMapper mapper)
    {
        _db = db;
        _mapperConfig = mapperConfig;
        _mapper = mapper;
    }//ScenariosController

    public IActionResult Index()
    {
        return RedirectToAction(nameof(LocalizedProducts));
    }//Index

    public async Task<IActionResult> LocalizedProducts(string culture = "en", CancellationToken cancellationToken = default)
    {
        var isSindhi = string.Equals(culture, "sd", StringComparison.OrdinalIgnoreCase);
        var projectionName = isSindhi ? ProductProjectionNames.Sindhi : ProductProjectionNames.English;

        var products = await _db.Products
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ProjectAs<ProductLocalizedDto>(_mapperConfig, projectionName)
            .ToListAsync(cancellationToken);

        ViewData["Culture"] = isSindhi ? "sd" : "en";
        ViewData["ProjectionName"] = projectionName;
        return View(products);
    }//LocalizedProducts

    public async Task<IActionResult> StaticConfigCall(CancellationToken cancellationToken = default)
    {
        var products = await _db.Products
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ProjectAs<ProductLocalizedDto>(AppMappingConfig.StaticConfigMethod(), ProductProjectionNames.Sindhi)
            .ToListAsync(cancellationToken);

        ViewData["ProjectionName"] = ProductProjectionNames.Sindhi;
        return View("LocalizedProducts", products);
    }//StaticConfigCall

    public async Task<IActionResult> RuntimeVsProjection(CancellationToken cancellationToken = default)
    {
        var product = await _db.Products.AsNoTracking().OrderBy(x => x.Id).FirstAsync(cancellationToken);

        var model = new RuntimeProjectionCompareDto
        {
            RuntimeMapped = _mapper.Map<Product, ProductRuntimeProjectionDto>(product)!,
            Projected = await _db.Products
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ProjectAs<ProductRuntimeProjectionDto>(_mapperConfig)
                .FirstAsync(cancellationToken)
        };

        return View(model);
    }//RuntimeVsProjection

    public async Task<IActionResult> SalaryIncrement(decimal incrementPercent = 10m, CancellationToken cancellationToken = default)
    {
        var scenarioConfig = EmployeeScenarioConfig.CreateSalaryProjectionConfig(incrementPercent);

        var employees = await _db.Employees
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ProjectAs<EmployeeSalaryDto>(scenarioConfig)
            .ToListAsync(cancellationToken);

        ViewData["IncrementPercent"] = incrementPercent;
        return View(employees);
    }//SalaryIncrement

    public async Task<IActionResult> PrefixSuffix(string prefix = "Mr. ", string suffix = " Sahib", CancellationToken cancellationToken = default)
    {
        var scenarioConfig = ProductScenarioConfig.CreatePrefixSuffixProjectionConfig(prefix, suffix);

        var products = await _db.Products
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ProjectAs<ProductPrefixSuffixDto>(scenarioConfig)
            .ToListAsync(cancellationToken);

        ViewData["Prefix"] = prefix;
        ViewData["Suffix"] = suffix;
        return View(products);
    }//PrefixSuffix
}//ScenariosController

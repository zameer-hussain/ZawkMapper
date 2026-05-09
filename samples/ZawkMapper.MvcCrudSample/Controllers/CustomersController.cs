using Microsoft.AspNetCore.Mvc;
using ZawkMapper.MvcCrudSample.Dtos;
using ZawkMapper.MvcCrudSample.Services;

namespace ZawkMapper.MvcCrudSample.Controllers;

public sealed class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(cancellationToken);
        return View(customers);
    }

    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetDetailsAsync(id, cancellationToken);
        if (customer is null) return NotFound();
        return View(customer);
    }

    public IActionResult Create()
    {
        return View(new CustomerCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(dto);

        await _customerService.CreateAsync(dto, cancellationToken);
        TempData["Success"] = "Customer created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetForEditAsync(id, cancellationToken);
        if (customer is null) return NotFound();
        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CustomerEditDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(dto);

        var updated = await _customerService.UpdateAsync(dto, cancellationToken);
        if (!updated) return NotFound();

        TempData["Success"] = "Customer updated successfully using raw parameterized SQL.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var deleted = await _customerService.DeleteAsync(id, cancellationToken);
        TempData[deleted ? "Success" : "Error"] = deleted ? "Customer soft deleted using raw parameterized SQL." : "Customer was not found.";
        return RedirectToAction(nameof(Index));
    }
}

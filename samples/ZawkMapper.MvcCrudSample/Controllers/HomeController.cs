using Microsoft.AspNetCore.Mvc;

namespace ZawkMapper.MvcCrudSample.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Customers");
}

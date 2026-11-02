using Microsoft.AspNetCore.Mvc;
using TechRepair.Services;

namespace TechRepair.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboard;
    public HomeController(IDashboardService dashboard) => _dashboard = dashboard;

    public async Task<IActionResult> Index()
    {
        var vm = await _dashboard.GetAsync();
        ViewData["Title"] = "Dashboard";
        return View(vm);
    }

    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacidade";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new Models.ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}

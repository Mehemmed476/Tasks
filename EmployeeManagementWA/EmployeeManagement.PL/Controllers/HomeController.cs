using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
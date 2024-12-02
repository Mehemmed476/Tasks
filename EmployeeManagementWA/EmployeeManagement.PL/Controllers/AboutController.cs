using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Controllers;

public class AboutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
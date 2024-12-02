using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Controllers;

public class ServiceController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
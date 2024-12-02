using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Controllers;

public class ContactController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
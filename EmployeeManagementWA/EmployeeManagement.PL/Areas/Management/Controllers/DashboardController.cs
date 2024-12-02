using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Areas.Management.Controllers;
[Area("Management")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
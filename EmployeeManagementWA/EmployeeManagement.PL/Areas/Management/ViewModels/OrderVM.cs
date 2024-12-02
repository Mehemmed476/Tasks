using EmployeeManagement.CORE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.PL.Areas.Management.ViewModels;

public class OrderVM
{
    public Order Order { get; set; }
    public IEnumerable<SelectListItem>? Services { get; set; }
    public IEnumerable<SelectListItem>? Masters { get; set; }
}
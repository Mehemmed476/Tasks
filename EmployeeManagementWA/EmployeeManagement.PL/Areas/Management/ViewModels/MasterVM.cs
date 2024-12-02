using EmployeeManagement.CORE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.PL.Areas.Management.ViewModels;

public class MasterVM
{
    public Master Master { get; set; }
    public IEnumerable<SelectListItem>? Services { get; set; }
}
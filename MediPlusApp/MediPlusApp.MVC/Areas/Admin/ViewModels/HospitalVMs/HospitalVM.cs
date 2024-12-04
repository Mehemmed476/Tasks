using MediPlusApp.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediPlusApp.MVC.Areas.Admin.ViewModels.HospitalVMs;

public class HospitalVM
{
    public Hospital? Hospital { get; set; }
    public IEnumerable<SelectListItem>? Doctors { get; set; }
    public List<int>? DoctorIds { get; set; }
}
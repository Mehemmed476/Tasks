using MediPlusApp.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediPlusApp.MVC.Areas.Admin.ViewModels.AppointmentsViewModels;

public class AppointmentVM
{
    public Appointment Appointment { get; set; }
    public IEnumerable<SelectListItem>? SelectedDoctor { get; set; }
    public IEnumerable<SelectListItem>? SelectedPatient { get; set; } 
}
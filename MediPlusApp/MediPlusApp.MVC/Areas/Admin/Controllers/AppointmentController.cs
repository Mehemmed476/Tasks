using MediPlus.BL.Services.Abstractions;
using MediPlusApp.DAL.DTOs.HospitalManagementSystemDTOs;
using MediPlusApp.DAL.Models;
using MediPlusApp.MVC.Areas.Admin.ViewModels.AppointmentsViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MediPlusApp.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class AppointmentController : Controller
{
    private readonly IGenericCRUDService _service;
    IWebHostEnvironment _webHostEnvironment;
    
    public AppointmentController(IGenericCRUDService service, IWebHostEnvironment webHostEnvironment)
    {
        _service = service;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Appointment> appointments = await _service.GetAllAsync<Appointment>();
        List<Appointment> activeAppointments = appointments.Where(s => !s.IsDeleted).ToList();
        foreach (Appointment appointment in activeAppointments)
        {
            appointment.Doctor = await _service.GetByIdAsync<Doctor>(appointment.DoctorId);
            appointment.Patient = await _service.GetByIdAsync<Patient>(appointment.PatientId);
        }
        
        return View(activeAppointments);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Appointment> appointments = await _service.GetAllAsync<Appointment>();
        List<Appointment> deactiveAppointments = appointments.Where(s => s.IsDeleted).ToList();
        foreach (Appointment appointment in deactiveAppointments)
        {
            appointment.Doctor = await _service.GetByIdAsync<Doctor>(appointment.DoctorId);
            appointment.Patient = await _service.GetByIdAsync<Patient>(appointment.PatientId);
        }
        return View(deactiveAppointments); 
    }

    public async Task<IActionResult> Details(int id)
    { 
        Appointment appointment = await _service.GetByIdAsync<Appointment>(id);
        appointment.Doctor = await _service.GetByIdAsync<Doctor>(appointment.DoctorId);
        appointment.Patient = await _service.GetByIdAsync<Patient>(appointment.PatientId);
       return View(appointment);
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        var model = new AppointmentVM
        {
            SelectedDoctor = Task.Run(() => _service.GetAllAsync<Doctor>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }),
            SelectedPatient = Task.Run(() => _service.GetAllAsync<Patient>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            })
        };
        
        return View(model);
    }
    
    [HttpPost]

    public async Task<IActionResult> Create(AppointmentVM appointmentVm)
    {
        var model = new AppointmentVM
        {
            SelectedDoctor = Task.Run(() => _service.GetAllAsync<Doctor>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }),
            SelectedPatient = Task.Run(() => _service.GetAllAsync<Patient>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            })
        };
        
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid slider item");
            return View(model);
        }
        
        IEnumerable<Appointment> appointments = await _service.GetAllAsync<Appointment>();
        Appointment lastAppointment = appointments.OrderByDescending(x => x.Id).First();
        
        if (appointmentVm.Appointment.AppointmentDate < DateTime.Now)
        {
            ModelState.AddModelError("Appointment.AppointmentDate", $"Appointment Date cannot be in the past");
            return View(model);
        }
        
        if (appointmentVm.Appointment.AppointmentDate < lastAppointment.AppointmentDate.AddMinutes(15) && appointmentVm.Appointment.DoctorId == lastAppointment.DoctorId)
        {
            ModelState.AddModelError("Appointment.AppointmentDate", $"Appointment Date must be after 15 minutes, You can add {lastAppointment.AppointmentDate.AddMinutes(15)}");
            return View(model);
        }
        
        Appointment appointment = new Appointment()
        {
           DoctorId = appointmentVm.Appointment.DoctorId,
           PatientId = appointmentVm.Appointment.PatientId,
           AppointmentDate = appointmentVm.Appointment.AppointmentDate
        };
        
        await _service.CreateAsync(appointment);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Appointment thisAppointment = await _service.GetByIdAsync<Appointment>(id);
        var model = new AppointmentVM()
        {
            SelectedDoctor = Task.Run(() => _service.GetAllAsync<Doctor>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }),
            SelectedPatient = Task.Run(() => _service.GetAllAsync<Patient>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }),
            Appointment = thisAppointment
        };
        return View(model);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, AppointmentVM appointmentVm)
    {
        if (!ModelState.IsValid)
        {
            Appointment thisAppointment = await _service.GetByIdAsync<Appointment>(id);
            var model = new AppointmentVM()
            {
                SelectedDoctor = Task.Run(() => _service.GetAllAsync<Doctor>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name + " " + x.Surname
                }),
                SelectedPatient = Task.Run(() => _service.GetAllAsync<Patient>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name + " " + x.Surname
                }),
                Appointment = thisAppointment
            }; 
            return View(model);
        }

        var existingAppointment = await _service.GetByIdAsync<Appointment>(id);
        if (existingAppointment == null)
        {
            return NotFound();
        }
    
        existingAppointment.DoctorId = appointmentVm.Appointment.DoctorId;
        existingAppointment.PatientId = appointmentVm.Appointment.PatientId;
        existingAppointment.AppointmentDate = appointmentVm.Appointment.AppointmentDate;
        

        await _service.UpdateAsync(existingAppointment);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Appointment>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Appointment>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Appointment>(id);
        return RedirectToAction(nameof(Trash));
    }
}
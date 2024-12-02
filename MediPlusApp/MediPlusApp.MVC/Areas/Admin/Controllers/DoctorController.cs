using MediPlus.BL.Services.Abstractions;
using MediPlusApp.DAL.DTOs.HomePageItemDTOs;
using MediPlusApp.DAL.DTOs.HospitalManagementSystemDTOs;
using MediPlusApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class DoctorController : Controller
{
    private readonly IGenericCRUDService _service;
    IWebHostEnvironment _webHostEnvironment;
    
    public DoctorController(IGenericCRUDService service, IWebHostEnvironment webHostEnvironment)
    {
        _service = service;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Doctor> doctors = await _service.GetAllAsync<Doctor>();
        List<Doctor> activeDoctors = doctors.Where(s => !s.IsDeleted).ToList();
        return View(activeDoctors);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Doctor> doctors = await _service.GetAllAsync<Doctor>();
        List<Doctor> deactiveDoctors = doctors.Where(s => s.IsDeleted).ToList();
        return View(deactiveDoctors); 
    }

    public async Task<IActionResult> Details(int id)
    {
        Doctor doctor = await _service.GetByIdAsync<Doctor>(id);
       return View(doctor);
    }
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(DoctorDto doctorDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid slider item");
            return View(doctorDto);
        }

        Doctor doctor = new Doctor()
        {
            Name = doctorDto.Name,
            Surname = doctorDto.Surname,
            FinCode = doctorDto.FinCode,
            PhoneNumber = doctorDto.PhoneNumber,
            Email = doctorDto.Email,
            UserName = $"{doctorDto.Name}{doctorDto.FinCode}"
        };
        
        await _service.CreateAsync(doctor);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Doctor doctor = await _service.GetByIdAsync<Doctor>(id);
        DoctorDto doctorDto = new DoctorDto()
        {
            Name = doctor.Name,
            Surname = doctor.Surname,
            FinCode = doctor.FinCode,
            PhoneNumber = doctor.PhoneNumber,
            Email = doctor.Email,
        };
        return View(doctorDto);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, DoctorDto doctorDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, provide valid slider item details");
            return View(doctorDto);
        }

        var existingDoctor = await _service.GetByIdAsync<Doctor>(id);
        if (existingDoctor == null)
        {
            return NotFound();
        }

        existingDoctor.Name = doctorDto.Name;
        existingDoctor.Surname = doctorDto.Surname;
        existingDoctor.FinCode = doctorDto.FinCode;
        existingDoctor.PhoneNumber = doctorDto.PhoneNumber;
        existingDoctor.Email = doctorDto.Email;
        existingDoctor.UserName = $"{doctorDto.Name}{doctorDto.FinCode}";
        

        await _service.UpdateAsync(existingDoctor);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Doctor>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Doctor>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Doctor>(id);
        return RedirectToAction(nameof(Trash));
    }

    public async Task<IActionResult> BeDeActiveDoctor(int id)
    {
        Doctor doctor = await _service.GetByIdAsync<Doctor>(id);
        doctor.IsDeleted = false;
        await _service.UpdateAsync(doctor);
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> BeActiveDoctor(int id)
    {
        Doctor doctor = await _service.GetByIdAsync<Doctor>(id);
        doctor.IsDeleted = true;
        await _service.UpdateAsync(doctor);
        return RedirectToAction(nameof(Index)); 
    } 
}
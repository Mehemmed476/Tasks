using MediPlus.BL.Services.Abstractions;
using MediPlusApp.DAL.DTOs.HospitalManagementSystemDTOs;
using MediPlusApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class PatientController : Controller
{
    private readonly IGenericCRUDService _service;
    IWebHostEnvironment _webHostEnvironment;
    
    public PatientController(IGenericCRUDService service, IWebHostEnvironment webHostEnvironment)
    {
        _service = service;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Patient> patients = await _service.GetAllAsync<Patient>();
        List<Patient> activePatients = patients.Where(s => !s.IsDeleted).ToList();
        return View(activePatients);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Patient> patients = await _service.GetAllAsync<Patient>();
        List<Patient> deactivePatients = patients.Where(s => s.IsDeleted).ToList();
        return View(deactivePatients); 
    }

    public async Task<IActionResult> Details(int id)
    {
        Patient patient = await _service.GetByIdAsync<Patient>(id);
       return View(patient);
    }
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(PatientDto patientDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid Patient");
            return View(patientDto);
        }

        Patient patient = new Patient()
        {
            Name = patientDto.Name,
            Surname = patientDto.Surname,
            FinCode = patientDto.FinCode,
            PhoneNumber = patientDto.PhoneNumber,
            Email = patientDto.Email,
            UserName = $"{patientDto.Name}{patientDto.FinCode}"
        };
        
        await _service.CreateAsync(patient);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Patient patient = await _service.GetByIdAsync<Patient>(id);
        PatientDto patientDto = new PatientDto()
        {
            Name = patient.Name,
            Surname = patient.Surname,
            FinCode = patient.FinCode,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
        };
        return View(patientDto);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, PatientDto patientDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, provide valid slider item details");
            return View(patientDto);
        }

        var existingPatient = await _service.GetByIdAsync<Patient>(id);
        if (existingPatient == null)
        {
            return NotFound();
        }

        existingPatient.Name = patientDto.Name;
        existingPatient.Surname = patientDto.Surname;
        existingPatient.FinCode = patientDto.FinCode;
        existingPatient.PhoneNumber = patientDto.PhoneNumber;
        existingPatient.Email = patientDto.Email;
        existingPatient.UserName = $"{patientDto.Name}{patientDto.FinCode}";
        

        await _service.UpdateAsync(existingPatient);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Patient>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Patient>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Patient>(id);
        return RedirectToAction(nameof(Trash));
    }
}
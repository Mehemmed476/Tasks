using MediPlus.BL.Services.Abstractions;
using MediPlusApp.DAL.Models;
using MediPlusApp.MVC.Areas.Admin.ViewModels.HospitalVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediPlusApp.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MediPlusApp.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class HospitalController : Controller
{
    private readonly IGenericCRUDService _service;
    private readonly MediPlusDbContext _context;

    public HospitalController(IGenericCRUDService service, MediPlusDbContext context)
    {
        _service = service;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<Hospital> hospitals = await _context.Hospitals.Where(a => !a.IsDeleted).Include(a => a.HospitalDoctors).ThenInclude(hd => hd.Doctor).ToListAsync();
        
        return View(hospitals);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details()
    {
        List<Hospital> hospitals = await _context.Hospitals.Include(a => a.HospitalDoctors).ThenInclude(hd => hd.Doctor).ToListAsync();
        
        return View(hospitals);
    }
    
    [HttpGet]
    public async Task<IActionResult> Trash()
    {
        List<Hospital> hospitals = await _context.Hospitals.Where(a => a.IsDeleted).Include(a => a.HospitalDoctors).ThenInclude(hd => hd.Doctor).ToListAsync();
        
        return View(hospitals);
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new HospitalVM()
        {
            Doctors = _service.GetAllAsync<Doctor>().Result.Where(x => !x.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }),
        };
        return View(model);
    }

    [HttpPost]

    public async Task<IActionResult> Create(HospitalVM hospitalVm)
    {
        if (!ModelState.IsValid)
        {
            var model = new HospitalVM()
            {
                Doctors = _service.GetAllAsync<Doctor>().Result.Where(x => !x.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name + " " + x.Surname
                }),
                Hospital = hospitalVm.Hospital
            };
            
            return View(model);
        }

        Hospital hospital = new Hospital()
        {
            Title = hospitalVm.Hospital.Title,
            Description = hospitalVm.Hospital.Description,
            Address = hospitalVm.Hospital.Address
        };
        
        await _service.CreateAsync(hospital);

        foreach (var item in hospitalVm.DoctorIds)
        {
            HospitalDoctor hospitalDoctor = new HospitalDoctor()
            {
                DoctorId = item,
                HospitalId = hospital.Id,
            };
            
            await _service.CreateAsync(hospitalDoctor);
        }
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Update()
    {
        var model = new HospitalVM()
        {
            Doctors = _service.GetAllAsync<Doctor>().Result.Where(x => !x.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name + " " + x.Surname
            }),
        };
        return View(model);
    }

    [HttpPost]

    public async Task<IActionResult> Update(HospitalVM hospitalVm)
    {
        if (!ModelState.IsValid)
        {
            var model = new HospitalVM()
            {
                Doctors = _service.GetAllAsync<Doctor>().Result.Where(x => !x.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name + " " + x.Surname
                }),
                Hospital = hospitalVm.Hospital
            };
            
            return View(model);
        }

        Hospital hospital = new Hospital()
        {
            Title = hospitalVm.Hospital.Title,
            Description = hospitalVm.Hospital.Description,
            Address = hospitalVm.Hospital.Address
        };
        
        await _service.CreateAsync(hospital);

        foreach (var item in hospitalVm.DoctorIds)
        {
            HospitalDoctor hospitalDoctor = new HospitalDoctor()
            {
                DoctorId = item,
                HospitalId = hospital.Id,
            };
            
            await _service.CreateAsync(hospitalDoctor);
        }
        
        return RedirectToAction("Index");
    }
    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Hospital>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Hospital>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Hospital>(id);
        return RedirectToAction(nameof(Trash));
    }
}
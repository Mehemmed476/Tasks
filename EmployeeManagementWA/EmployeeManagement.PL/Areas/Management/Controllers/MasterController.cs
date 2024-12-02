using EmployeeManagement.BL.Services.Abstractions;
using EmployeeManagement.CORE.Models;
using EmployeeManagement.PL.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.PL.Areas.Management.Controllers;
[Area("Management")]
public class MasterController : Controller
{
    private readonly IGenericCRUDService _service;


    public MasterController(IGenericCRUDService service)
    {
        _service = service;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Master> masters = await _service.GetAllAsync<Master>();
        List<Master> activeMasters = masters.Where(s => !s.IsDeleted).ToList();
        foreach (Master master in activeMasters)
        {
            master.Service = await _service.GetByIdAsync<Service>(master.ServiceId);
        }
        
        return View(activeMasters);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Master> masters = await _service.GetAllAsync<Master>();
        List<Master> deactiveMasters = masters.Where(s => s.IsDeleted).ToList();
        foreach (Master master in deactiveMasters)
        {
            master.Service = await _service.GetByIdAsync<Service>(master.ServiceId);
        }
        return View(deactiveMasters); 
    }

    public async Task<IActionResult> Details(int id)
    {
        Master master = await _service.GetByIdAsync<Master>(id);
        master.Service = await _service.GetByIdAsync<Service>(master.ServiceId);
       return View(master);
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        var model = new MasterVM
        {
            Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            })
        };
        
        return View(model);
    }
    
    [HttpPost]

    public async Task<IActionResult> Create(MasterVM masterVm)
    {
        var model = new MasterVM
        {
            Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            })
        };
        
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid slider item");
            return View(model);
        }
        
        Master master = new Master()
        {
           FirstName = masterVm.Master.FirstName,
           LastName = masterVm.Master.LastName,
           Email = masterVm.Master.Email,
           PhoneNumber = masterVm.Master.PhoneNumber,
           UserName = masterVm.Master.UserName,
           ExperienceYear = masterVm.Master.ExperienceYear,
           IsActive = masterVm.Master.IsActive,
           ServiceId = masterVm.Master.ServiceId
        };
        
        await _service.CreateAsync(master);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Master thisMaster = await _service.GetByIdAsync<Master>(id);
        var model = new MasterVM()
        {
            Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }),
            Master = thisMaster
        };
        return View(model);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, MasterVM masterVm)
    {
        if (!ModelState.IsValid)
        {
            Master thisMaster = await _service.GetByIdAsync<Master>(id);
            var model = new MasterVM()
            {
                Services = Task.Run(() => _service.GetAllAsync<Service>()).Result.Where(d => d.IsActive && !d.IsDeleted).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }),
                Master = thisMaster
            }; 
            return View(model);
        }

        var existingMaster = await _service.GetByIdAsync<Master>(id);
        if (existingMaster == null)
        {
            return NotFound();
        }
    
        existingMaster.FirstName = masterVm.Master.FirstName;
        existingMaster.LastName = masterVm.Master.LastName;
        existingMaster.Email = masterVm.Master.Email;
        existingMaster.PhoneNumber = masterVm.Master.PhoneNumber;
        existingMaster.UserName = masterVm.Master.UserName;
        existingMaster.ExperienceYear = masterVm.Master.ExperienceYear;
        existingMaster.IsActive = masterVm.Master.IsActive;
        existingMaster.ServiceId = masterVm.Master.ServiceId;
        

        await _service.UpdateAsync(existingMaster);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Master>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Master>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Master>(id);
        return RedirectToAction(nameof(Trash));
    }
}
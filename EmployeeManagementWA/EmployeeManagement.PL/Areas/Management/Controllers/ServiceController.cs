using EmployeeManagement.BL.Services.Abstractions;
using EmployeeManagement.CORE.Models;
using EmployeeManagement.PL.Areas.Management.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Areas.Management.Controllers;
[Area("Management")]
public class ServiceController : Controller
{
    private readonly IGenericCRUDService _service;


    public ServiceController(IGenericCRUDService service)
    {
        _service = service;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Service> services = await _service.GetAllAsync<Service>();
        List<Service> activeServices = services.Where(s => !s.IsDeleted).ToList();
        return View(activeServices);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Service> services = await _service.GetAllAsync<Service>();
        List<Service> deactiveServices = services.Where(s => s.IsDeleted).ToList();
        return View(deactiveServices); 
    }

    public async Task<IActionResult> Details(int id)
    {
        Service service = await _service.GetByIdAsync<Service>(id);
       return View(service);
    }
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    
    public async Task<IActionResult> Create(Service service)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid slider item");
            return View(service);
        }
        
        await _service.CreateAsync(service);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Service service = await _service.GetByIdAsync<Service>(id);
        return View(service);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, Service service)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, provide valid slider item details");
            return View(service);
        }

        var existingService = await _service.GetByIdAsync<Service>(id);
        if (existingService is null)
        {
            return NotFound();
        }

        existingService.Title = service.Title;
        existingService.Description = service.Description;
        existingService.IsActive = service.IsActive;
        
        await _service.UpdateAsync(existingService);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Service>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Service>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Service>(id);
        return RedirectToAction(nameof(Trash));
    }

    public async Task<IActionResult> BeDeActiveService(int id)
    {
        Service service = await _service.GetByIdAsync<Service>(id);
        service.IsDeleted = false;
        await _service.UpdateAsync(service);
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> BeActiveService(int id)
    {
        Service service = await _service.GetByIdAsync<Service>(id);
        service.IsDeleted = true;
        await _service.UpdateAsync(service);
        return RedirectToAction(nameof(Index)); 
    }
}
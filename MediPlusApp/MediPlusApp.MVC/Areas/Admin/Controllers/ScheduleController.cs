using MediPlus.BL.Services.Abstractions;
using MediPlusApp.DAL.DTOs.HomePageItemDTOs;
using MediPlusApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class ScheduleController : Controller
{
    private readonly IGenericCRUDService _service;
    IWebHostEnvironment _webHostEnvironment;
    
    public ScheduleController(IGenericCRUDService service, IWebHostEnvironment webHostEnvironment)
    {
        _service = service;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Schedule> schedules = await _service.GetAllAsync<Schedule>();
        List<Schedule> activeSchedules = schedules.Where(s => !s.IsDeleted).ToList();
        return View(activeSchedules);
    }

    [HttpGet]

    public async Task<IActionResult> Trash()
    {
        IEnumerable<Schedule> sliderItems = await _service.GetAllAsync<Schedule>();
        List<Schedule> deactiveSliderItems = sliderItems.Where(s => s.IsDeleted).ToList();
        return View(deactiveSliderItems); 
    }

    public async Task<IActionResult> Details(int id)
    {
        Schedule sliderItem = await _service.GetByIdAsync<Schedule>(id);
       return View(sliderItem);
    }
    [HttpGet]
    
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(ScheduleDto scheduleDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, choose a valid schedule");
            return View(scheduleDto);
        }
        
        Schedule schedule = new Schedule()
        {
            Subject = scheduleDto.Subject,
            Title = scheduleDto.Title,
            Description = scheduleDto.Description,
            ButtonUrl = scheduleDto.ButtonUrl
        };
        
        schedule.CreatedDate = DateTime.Now;
        
        await _service.CreateAsync(schedule);
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]

    public async Task<IActionResult> Update(int id)
    {
        Schedule schedule = await _service.GetByIdAsync<Schedule>(id);
        
        ScheduleDto sliderItemDto = new ScheduleDto()
        {
            Subject = schedule.Subject,
            Title = schedule.Title,
            Description = schedule.Description,
            ButtonUrl = schedule.ButtonUrl
        };
        return View(sliderItemDto);
    }

    [HttpPost]
    
    public async Task<IActionResult> Update(int id, ScheduleDto scheduleDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please, provide valid slider item details");
            return View(scheduleDto);
        }

        var existingSchedule = await _service.GetByIdAsync<Schedule>(id);
        if (existingSchedule == null)
        {
            return NotFound();
        }
        
        existingSchedule.Subject = scheduleDto.Subject;
        existingSchedule.Title = scheduleDto.Title;
        existingSchedule.Description = scheduleDto.Description;
        existingSchedule.ButtonUrl = scheduleDto.ButtonUrl;

        await _service.UpdateAsync(existingSchedule);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> SoftDelete(int id)
    {
        await _service.SoftDeleteAsync<Schedule>(id);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync<Schedule>(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]

    public async Task<IActionResult> Restore(int id)
    {
        await _service.RestoreAsync<Schedule>(id);
        return RedirectToAction(nameof(Trash));
    }
}
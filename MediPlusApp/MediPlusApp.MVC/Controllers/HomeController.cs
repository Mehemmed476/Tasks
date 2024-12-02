using MediPlus.BL.Services.Abstractions;
using MediPlusApp.DAL.Models;
using MediPlusApp.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MediPlusApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IGenericCRUDService _service;
    
    public HomeController(IGenericCRUDService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<SliderItem> sliderItems = await _service.GetAllAsync<SliderItem>();
        List<SliderItem> threeSliderItems = sliderItems.OrderByDescending(x => x.Id).Take(3).ToList();
        IEnumerable<Schedule> schedules = await _service.GetAllAsync<Schedule>();
        List<Schedule> threeSchedules = schedules.OrderByDescending(x => x.Id).Take(3).ToList();
        HomePageVM homePageVM = new()
        {
            SliderItems = threeSliderItems,
            Schedules = threeSchedules
        };
        return View(homePageVM);
    }
}
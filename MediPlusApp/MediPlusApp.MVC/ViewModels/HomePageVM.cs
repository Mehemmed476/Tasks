using MediPlusApp.DAL.Models;

namespace MediPlusApp.MVC.ViewModels;

public class HomePageVM
{
    public List<SliderItem> SliderItems { get; set; }
    public List<Schedule> Schedules { get; set; }
}
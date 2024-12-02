using MediPlusApp.DAL.Models.Base;

namespace MediPlusApp.DAL.Models;

public class SliderItem : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string ImageUrl { get; set; }
}
using MediPlusApp.DAL.Models.Base;

namespace MediPlusApp.DAL.Models;

public class Schedule : BaseAuditableEntity
{
    public string Subject { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ButtonUrl { get; set; }
}
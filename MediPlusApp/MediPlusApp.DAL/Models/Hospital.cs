using MediPlusApp.DAL.Models.Base;

namespace MediPlusApp.DAL.Models;

public class Hospital : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public ICollection<HospitalDoctor>? HospitalDoctors { get; set; }
}
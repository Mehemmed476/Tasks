using System.ComponentModel.DataAnnotations;
using MediPlusApp.DAL.Models.Base;

namespace MediPlusApp.DAL.Models;

public class Doctor : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FinCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public ICollection<Appointment>? Appointments { get; set; }
}
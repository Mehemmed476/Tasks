using EmployeeManagement.CORE.Models.Base;

namespace EmployeeManagement.CORE.Models;

public class Master : BaseAuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public int ExperienceYear { get; set; }
    public bool IsActive { get; set; } = true;
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
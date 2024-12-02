using EmployeeManagement.CORE.Models.Base;

namespace EmployeeManagement.CORE.Models;

public class Service : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Master>? Masters { get; set; }
    public ICollection<Order>? Orders { get; set; } 
}
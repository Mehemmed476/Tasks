using EmployeeManagement.CORE.Models.Base;

namespace EmployeeManagement.CORE.Models;

public class Order : BaseAuditableEntity
{
    public string ClientName { get; set; }
    public string ClientSurName { get; set; }
    public string ClientPhoneNumber { get; set; }
    public string ClientEmail { get; set; }
    public string Problem { get; set; }
    public bool IsActive { get; set; } = true;
    public int ServiceId { get; set; }
    public Service? Service { get; set; }
    public int MasterId { get; set; }
    public Master? Master { get; set; }
}

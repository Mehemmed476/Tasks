namespace MediPlusApp.DAL.Models.Base;

public class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedDate { get; set; }
    public bool IsActive { get; set; } = true;
}
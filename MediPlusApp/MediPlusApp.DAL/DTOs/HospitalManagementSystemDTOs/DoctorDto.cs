namespace MediPlusApp.DAL.DTOs.HospitalManagementSystemDTOs;

public record DoctorDto()
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string FinCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
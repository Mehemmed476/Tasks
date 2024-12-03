using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.CORE.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
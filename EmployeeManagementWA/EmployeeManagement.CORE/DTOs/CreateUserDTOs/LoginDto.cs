using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.CORE.DTOs.CreateUserDTOs;

public class LoginDto
{
    [Required, Display(Prompt = "Username or Email...")]
    public string UsernameOrEmail { get; set; }
    [Required, Display(Prompt = "Password..."), DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
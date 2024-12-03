using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.CORE.DTOs.CreateUserDTOs;

public class RegisterDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Display(Prompt = "FirstName")]
    public string FirstName { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [Display(Prompt = "LastName")]
    public string LastName { get; set; }
    [Required] 
    [DataType(DataType.EmailAddress)]
    [Display(Prompt = "Email")]
    public string Email { get; set; }
    [Required]
    [Length(3,30)]
    [Display(Prompt = "UserName")]
    public string NickName { get; set; }
    [Required]
    [DataType(DataType.Password)] 
    [Display(Prompt = "Password")]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Prompt = "Password Again")]
    public string ConfirmPassword { get; set; }
}
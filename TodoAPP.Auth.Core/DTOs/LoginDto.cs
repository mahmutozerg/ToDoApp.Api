using System.ComponentModel.DataAnnotations;

namespace TodoAPP.Auth.Core.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Email field is required")]
    [EmailAddress(ErrorMessage = "Invlaid email format")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace TodoAPP.Auth.Core.DTOs;

public class CreateUserDto
{
    [EmailAddress] 
    [Required(ErrorMessage = "Email field is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "UserName field is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password field is required")]
    public string Password { get; set; }
}
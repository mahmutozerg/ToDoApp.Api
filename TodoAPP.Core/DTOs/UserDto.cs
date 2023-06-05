using System.ComponentModel.DataAnnotations;

namespace TodoAPP.Core.DTOs;

public class UserDto
{
    [Required(ErrorMessage = "Missing username field")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Missing email field")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Missing password field")]
    public string Password { get; set; }
}
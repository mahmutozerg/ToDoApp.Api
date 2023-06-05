using System.ComponentModel.DataAnnotations;

namespace TodoAPP.Core.DTOs;

public class UserLoginDto
{
    [Required(ErrorMessage = "Missing Login Credentials")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Missing Login Credentials")]
    public string Password { get; set; }

}
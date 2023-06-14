using System.ComponentModel.DataAnnotations;

namespace TodoAPP.Core.DTOs;

public class UserUpdateDto
{
    [MaxLength(50)]
    public string? UserName { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }

    public string?  Password { get; set; }
}
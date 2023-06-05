using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public class UserInfo
{
    
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    [JsonIgnore]
    public bool IsDeleted { get; set; }
    
    public UserInfoDetailed UserInfoDetailed { get; set; }
    [JsonIgnore]
    public List<ToDo> ToDos { get; set; } = new List<ToDo>();

}
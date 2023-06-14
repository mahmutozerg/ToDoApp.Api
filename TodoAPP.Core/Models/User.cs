using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public class User
{
    
    public int Id { get; set; }
   
    public string? UserName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    [JsonIgnore]
    public bool IsDeleted { get; set; }
    [JsonIgnore]
    public List<ToDo> ToDos { get; set; } = new List<ToDo>();

}
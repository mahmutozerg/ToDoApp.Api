using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public class UserEntity:BaseEntity
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    [JsonIgnore]
    public bool IsDeleted { get; set; }
    [JsonIgnore]
    public List<ToDoEntity> ToDos { get; set; } = new List<ToDoEntity>();

}
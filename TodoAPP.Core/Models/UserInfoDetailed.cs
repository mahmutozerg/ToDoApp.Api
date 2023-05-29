using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public  class UserInfoDetailed
{
    [Key]
    public int Id { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsUserActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public UserInfo UserInfo;
}
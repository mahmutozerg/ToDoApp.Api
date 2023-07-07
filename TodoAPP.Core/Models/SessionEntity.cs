using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public  class SessionEntity:BaseEntity
{
    public string Id { get; set; } = null!;

    public DateTime LastLogin { get; set; }
    public bool IsSessionActive { get; set; }
    public string SessionGUID { get; set; }
    public DateTime? EndedAt { get; set; }
    
    public UserEntity UserEntity;
    public string UserId;
}
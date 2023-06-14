using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public  class Session
{
    public int Id { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsSessionActive { get; set; }
    public string SessionGUID { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    
    public User User;
    public int UserId;
}
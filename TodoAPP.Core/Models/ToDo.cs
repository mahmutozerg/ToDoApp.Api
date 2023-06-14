using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public class ToDo
{
    public int Id { get; set; }
    public string ListName { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public string TodoContext { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueTime { get; set; }
    [JsonIgnore]
    public bool  IsDeleted { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public  User User { get; set; }
}
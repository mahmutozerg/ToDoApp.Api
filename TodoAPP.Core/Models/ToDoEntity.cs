using System.Text.Json.Serialization;

namespace TodoAPP.Core.Models;

public class ToDoEntity:BaseEntity
{
    public string Id { get; set; } = null!;

    public string ListName { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public string TodoContext { get; set; }
    public DateTime DueTime { get; set; }
    [JsonIgnore]
    public string UserId { get; set; }
    [JsonIgnore]
    public  UserEntity UserEntity { get; set; }
}
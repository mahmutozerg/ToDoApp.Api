namespace TodoAPP.Core.Models;

public class ToDo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
    public string TodoContext { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DueTime { get; set; }
    public bool  IsDeleted { get; set; }
    public int UserInfoId { get; set; }
}
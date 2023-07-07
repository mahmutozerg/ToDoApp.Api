namespace TodoAPP.Core.DTOs;

public class ToDoResponseDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string TodoContext { get; set; }
    public DateTime DueTime { get; set; }

    public bool IsDone { get; set; } 
}
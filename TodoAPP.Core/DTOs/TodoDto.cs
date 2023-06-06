namespace TodoAPP.Core.DTOs;

public class TodoDto
{
    public string ListName { get; set; } = string.Empty;
    public string Title { get; set; }= string.Empty;
    public string TodoContext { get; set; }= string.Empty;
    public DateTime DueTime { get; set; }

}
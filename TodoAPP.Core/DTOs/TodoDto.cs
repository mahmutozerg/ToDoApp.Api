namespace TodoAPP.Core.DTOs;

public class TodoDto
{
    public string ListName { get; set; }
    public string Title { get; set; }
    public string TodoContext { get; set; }
    public DateTime DueTime { get; set; }
    public string SessionID { get; set; }

}
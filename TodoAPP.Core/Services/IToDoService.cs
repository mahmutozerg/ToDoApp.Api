using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IToDoService:IService<ToDo>
{
    Task<List<ToDo?>?> GetAllAsync(string sessionId);
    Task<ToDo?> CreateToDo(TodoDto todoDto, string sessionId);
    Task<List<ToDo>?> GetBylistNameAsync(string listName, string sessionId);
    Task<ToDo?> UpdateToDoStatusById(string sessionId,int id);
    Task<List<ToDo?>?> GetCompletedToDos(string sessionId);
    Task<ToDo?> Update(TodoDto todoDto, int id, string sessionId);
    Task<ToDo?> Remove(string sessionId, int id);
}
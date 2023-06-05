using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IToDoService:IService<ToDo>
{
    Task<IQueryable<ToDo>> GetAllAsync();
    Task<ToDo> Create(TodoDto todoDto);
    Task<List<ToDo>> GetBylistNameAsync(string listName, string sessionId);
    Task<List<ToDo>> GetWithFullUserInfo(string sessionId);
    Task<ToDo> UpdateToDoStatusById(int id);
    Task<List<ToDo>> GetCompletedToDos(string sessionId);

}
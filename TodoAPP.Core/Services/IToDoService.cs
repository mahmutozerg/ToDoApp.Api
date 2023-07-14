using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IToDoService:IService<ToDo>
{
    Task<CustomResponseListDataDto<ToDoResponseDto>> GetAllAsync(string userId);
    Task<CustomResponseDto<ToDoResponseDto>> CreateToDo(TodoDto todoDto, string sessionId);
    Task<CustomResponseListDataDto<ToDoResponseDto>> GetBylistNameAsync(string listName, string userId);
    Task<CustomResponseDto<ToDoResponseDto>> UpdateToDoStatusById(string userId, string id);
    Task<CustomResponseListDataDto<ToDoResponseDto>> GetCompletedToDos(string userId);
    Task<CustomResponseDto<ToDoResponseDto>> Update(TodoDto todoDto, string id, string userId);
    Task<CustomResponseNoDataDto> Remove(string userId, string id);
}
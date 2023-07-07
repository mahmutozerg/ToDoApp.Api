using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IToDoService:IService<ToDoEntity>
{
    Task<CustomResponseListDataDto<ToDoResponseDto>> GetAllAsync(string sessionId);
    Task<CustomResponseDto<ToDoResponseDto>> CreateToDo(TodoDto todoDto, string sessionId);
    Task<CustomResponseListDataDto<ToDoResponseDto>> GetBylistNameAsync(string listName, string sessionId);
    Task<CustomResponseDto<ToDoResponseDto>> UpdateToDoStatusById(string sessionId, string id);
    Task<CustomResponseListDataDto<ToDoResponseDto>> GetCompletedToDos(string sessionId);
    Task<CustomResponseDto<ToDoResponseDto>> Update(TodoDto todoDto, string id, string sessionId);
    Task<CustomResponseNoDataDto> Remove(string sessionId, string id);
}
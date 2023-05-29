using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IToDoService:IService<ToDo>
{
    Task<List<ToDo>> GetAllAsync();

}
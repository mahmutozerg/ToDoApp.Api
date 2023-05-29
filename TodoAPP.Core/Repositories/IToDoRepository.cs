using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IToDoRepository:IGenericRepository<ToDo>
{
    Task<List<ToDo>> GetAllAsync();
}
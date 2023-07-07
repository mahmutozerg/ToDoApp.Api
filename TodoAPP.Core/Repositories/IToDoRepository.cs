using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IToDoRepository:IGenericRepository<ToDoEntity>
{
     Task<List<ToDoEntity>?> GetBylistNameAsync(string listName, string userId);


}
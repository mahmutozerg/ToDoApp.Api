using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IToDoRepository:IGenericRepository<ToDo>
{
     Task<List<ToDo>?> GetByListNameAsync(string listName, string userId);


}
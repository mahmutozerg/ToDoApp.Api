using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IToDoRepository:IGenericRepository<ToDo>
{
     Task<IQueryable<ToDo>> GetAllAsync();
     Task<List<ToDo>> GetBylistNameAsync(string listName, string sessionId);
     Task<List<ToDo>> GetWithFullUserInfo(string sessionId);

}
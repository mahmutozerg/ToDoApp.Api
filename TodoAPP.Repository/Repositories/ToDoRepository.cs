using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class ToDoRepository:GenericRepository<ToDo>,IToDoRepository
{
    private readonly DbSet<ToDo> _dbSet;

    public ToDoRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<ToDo>();
    }

    public async Task<List<ToDo>?> GetBylistNameAsync(string listName, int userId)
    {
        var filteredRelatedUserToDosList =
           await _dbSet.Where(t => t.ListName == listName && t.UserId == userId && !t.IsDeleted)
               .AsNoTracking().ToListAsync();
        return filteredRelatedUserToDosList;
    }

    
}
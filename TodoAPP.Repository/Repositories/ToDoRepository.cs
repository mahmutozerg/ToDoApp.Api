using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class ToDoRepository:GenericRepository<ToDoEntity>,IToDoRepository
{
    private readonly DbSet<ToDoEntity> _dbSet;

    public ToDoRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<ToDoEntity>();
    }

    public async Task<List<ToDoEntity>?> GetBylistNameAsync(string listName, string userId)
    {
        var filteredRelatedUserToDosList =
           await _dbSet.Where(t => t.ListName == listName && t.UserId == userId && !t.IsDeleted)
               .AsNoTracking().ToListAsync();
        return filteredRelatedUserToDosList;
    }

    
}
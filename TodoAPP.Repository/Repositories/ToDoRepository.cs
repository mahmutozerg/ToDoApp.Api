using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class ToDoRepository:GenericRepository<ToDo>,IToDoRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<ToDo> _dbSet;

    public ToDoRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<ToDo>();
    }

    public Task<IQueryable<ToDo>> GetAllAsync()
    {
        return Task.FromResult(_dbSet.AsNoTracking());
    }

    public async Task<List<ToDo>> GetBylistNameAsync(string listName, string sessionId)
    {
        var relatedUserToDos = await GetWithFullUserInfo(sessionId);
        return relatedUserToDos.Where(x => x.ListName == listName && !x.IsDeleted).ToList();
    }

    public Task<List<ToDo>> GetWithFullUserInfo(string sessionId)
    {
        return  Task.FromResult<List<ToDo>>(new List<ToDo>(_dbSet.Include(x => x.UserInfo).ThenInclude(x => x.UserInfoDetailed).AsNoTracking().
            Where(x => x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && !x.IsDeleted)));
    }


}
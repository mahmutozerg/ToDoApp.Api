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

    public async Task<ToDo?> GetByIdAsync(string sessionId,int id)
    {
        var todos = await _dbSet.Include(x => x.UserInfo)
            .ThenInclude(x => x.UserInfoDetailed)
            .Where(x=>x.UserInfo.UserInfoDetailed.SessionGUID == sessionId).Where(x=>x.Id == id)
            .FirstOrDefaultAsync();
        return todos;
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

    public async Task<ToDo> GetWithFullUserInfoById(string sessionId, int id)
    {
        return await _dbSet.Include(x => x.UserInfo).ThenInclude(x => x.UserInfoDetailed)
            .Where(x => x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && !x.IsDeleted&& x.Id == id).FirstOrDefaultAsync();

    }
}
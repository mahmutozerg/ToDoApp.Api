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
        var entities =  (_dbSet.AsNoTracking());

        return Task.FromResult(entities);
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
        var filteredRelatedUserToDosList = relatedUserToDos.Where(x => x.ListName == listName && !x.IsDeleted).ToList();
        return filteredRelatedUserToDosList;
    }

    public async Task<List<ToDo>> GetWithFullUserInfo(string sessionId)
    {
        var entities= await _dbSet.Include(x => x.UserInfo).ThenInclude(x => x.UserInfoDetailed).AsNoTracking()
            .Where(x => x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && !x.IsDeleted).ToListAsync();

        return entities;
    }

    public async Task<ToDo> GetWithFullUserInfoById(string sessionId, int id)
    {
        var entities = await _dbSet.Include(x => x.UserInfo).ThenInclude(x => x.UserInfoDetailed)
            .Where(x => x.UserInfo.UserInfoDetailed.SessionGUID == sessionId && !x.IsDeleted&& x.Id == id).FirstOrDefaultAsync();

        return entities;
    }
    
}
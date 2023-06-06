using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class UserInfoRepository:GenericRepository<UserInfo>,IUserInfoRepository
{    
    private readonly AppDbContext _context;
    private readonly DbSet<UserInfo> _dbSet;

    public UserInfoRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<UserInfo>();
    }

    public async Task<IQueryable<UserInfo>> GetAllUserInfoWithDetails()
    {
        return   _dbSet.Include(x=>x.UserInfoDetailed).AsNoTracking();
    }

    public async Task<IQueryable<UserInfo>> GetUserInfoWithDetailsById(int id)
    {
        return _dbSet.Include(x => x.UserInfoDetailed).Where(x => x.Id == id &&  !x.IsDeleted) ;
    }

    public Task<UserInfo> SetUserDetailInfo(UserInfo userInfo)
    {
        return _dbSet.Where(x => x.UserName == userInfo.UserName && !x.IsDeleted).SingleOrDefaultAsync();
    }

    public async Task<UserInfo?> GetUserInfoWithDetailsByUsername(string username)
    {
        return await _dbSet.Include(x=> x.UserInfoDetailed).Where(x => x.UserName == username && !x.IsDeleted).FirstOrDefaultAsync();
    }

    public async Task<UserInfo?> GetUserInfoWithDetailsByEmail(string email)
    {
        return await _dbSet.Include(x=> x.UserInfoDetailed).Where(x => x.Email == email && !x.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<UserInfo?> GetUserInfoWithDetailsBySessionId(string sessionId)
    {
        return await _dbSet.Include(x => x.UserInfoDetailed)
            .Where(x => x.UserInfoDetailed.SessionGUID == sessionId && !x.IsDeleted).FirstOrDefaultAsync();
    }


}
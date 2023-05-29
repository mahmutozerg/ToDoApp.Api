using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class UserInfoRepository:GenericRepository<UserInfo>,IUserInfoRepository
{
    
    public UserInfoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IQueryable<UserInfo>> GetAllUserInfoWithDetails()
    {
        return   _context.UserInfos.Include(x=>x.UserInfoDetailed);
    }

    public async Task<IQueryable<UserInfo>> GetUserInfoWithDetailsById(int id)
    {
        return _context.UserInfos.Include(x => x.UserInfoDetailed).Where(x => x.Id == id);
    }

    public Task<UserInfo> SetUserDetailInfo(UserInfo userInfo)
    {
        return _context.UserInfos.Where(x => x.UserName == userInfo.UserName).SingleOrDefaultAsync();
    }


}
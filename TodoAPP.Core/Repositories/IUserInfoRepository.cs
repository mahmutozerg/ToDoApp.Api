using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserInfoRepository:IGenericRepository<UserInfo>
{
    Task<IQueryable<UserInfo>> GetAllUserInfoWithDetails();
    Task<IQueryable<UserInfo>> GetUserInfoWithDetailsById(int id);
    Task<UserInfo?> SetUserDetailInfo(UserInfo userInfo);


}
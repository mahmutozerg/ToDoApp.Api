using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserInfoRepository:IGenericRepository<UserInfo>
{
    Task<IQueryable<UserInfo>> GetAllUserInfoWithDetails();
    Task<IQueryable<UserInfo>> GetUserInfoWithDetailsById(int id);
    Task<UserInfo?> SetUserDetailInfo(UserInfo userInfo);
    
    Task<UserInfo?> GetUserInfoWithDetailsByUsername(string username);
    Task<UserInfo?> GetUserInfoWithDetailsByEmail(string email);
    Task<UserInfo?> GetUserInfoWithDetailsBySessionId(string sessionId);
    
}
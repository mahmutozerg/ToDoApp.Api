using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserInfoRepository:IGenericRepository<UserInfo>
{
    
    Task<UserInfo?> GetUserInfoWithDetailsByEmail(string email);
    Task<UserInfo?> GetUserInfoWithDetailsBySessionId(string sessionId);
    
}
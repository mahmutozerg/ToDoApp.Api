using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserRepository:IGenericRepository<User>
{
    Task<User?> GetUserInfoByEmail(string email);
    
}
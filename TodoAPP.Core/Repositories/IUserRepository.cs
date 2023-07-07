using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserRepository:IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetUserInfoByEmail(string email);
    
}
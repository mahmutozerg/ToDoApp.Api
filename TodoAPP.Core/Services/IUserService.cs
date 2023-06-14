using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IUserService:IService<User>
{
    Task<User?> GetUserInfoByEmail(string email);
    
    Task<User?> RemoveAsync(User user);
    
    Task<string> UpdateUser(UserUpdateDto userUpdateDto, Session? session);
    

}
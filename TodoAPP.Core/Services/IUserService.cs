using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IUserService:IService<User>
{
    Task<CustomResponseDto<User>> GetUserInfoByEmail(string email);
    
    Task<CustomResponseDto<User>> RemoveAsync(User user);
    
    Task<CustomResponseDto<User>> UpdateUser(UserUpdateDto userUpdateDto, Session? session);
    

}
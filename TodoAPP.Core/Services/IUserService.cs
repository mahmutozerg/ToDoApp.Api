using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IUserService:IService<UserEntity>
{
    Task<CustomResponseDto<UserEntity>> GetUserInfoByEmail(string email);
    
    Task<CustomResponseDto<UserEntity>> RemoveAsync(UserEntity userEntity);
    
    Task<CustomResponseDto<UserEntity>> UpdateUser(UserUpdateDto userUpdateDto, SessionEntity? session);
    

}
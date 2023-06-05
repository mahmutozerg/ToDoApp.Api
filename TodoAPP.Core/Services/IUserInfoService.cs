using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IUserInfoService:IService<UserInfo>
{
    Task<string>Register(UserDto userDto,UserInfoDetailed userInfoDetailed);
    Task<string> Login(UserDto userDto);

    Task<IEnumerable<UserInfo>> GetAllUserInfoWithDetails();
    Task<IQueryable<UserInfo>> GetUserInfoWithDetailsById(int id);
    Task<string> CheckUserExist(UserDto userDto);

    Task<UserInfo?> GetUserInfoWithDetailsByUsername(string username);
    Task<UserInfo?> GetUserInfoWithDetailsByEmail(string email);
    Task<UserInfo?> GetUserInfoWithDetailsBySessionId(string sessionId);

}
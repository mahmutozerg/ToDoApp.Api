using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IAuthenticationService
{
    string CalculateHash(string password, string salt);
    Task<string> Register(UserDto userDto);
    Task<SessionEntity?> Login(UserDto userDto);
    Task<SessionEntity> Logout(SessionGuiddto sessionDto);


}
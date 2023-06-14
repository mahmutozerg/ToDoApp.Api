using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IAuthenticationService
{
    string CalculateHash(string password, string salt);
    Task<string> Register(UserDto userDto);
    Task<Session?> Login(UserDto userDto);
    Task<Session> Logout(SessionGuiddto sessionDto);


}
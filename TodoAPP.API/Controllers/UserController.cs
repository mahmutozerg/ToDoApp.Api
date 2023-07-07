using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Services;

namespace TodoAPP.API.Controllers;

public class UserController:CustomBaseController
{
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    
    public UserController(IUserService userService, ISessionService sessionService)
    {
        _userService = userService;
        _sessionService = sessionService;
    }
    
    [HttpDelete("[action]")]
    public async Task<IActionResult> Remove (UserLoginDto userLoginDto)
    {
        var userResponseDto = await _userService.GetUserInfoByEmail(userLoginDto.Email);

        if (userResponseDto.StatusCode == 404)
            return CreateActionResult(userResponseDto);
        
        var sessions = await _sessionService.Where(s=> s.UserId == userResponseDto.Data.Id).ToListAsync();
        await _sessionService.RemoveSessions(sessions);
        
        var userRemovalResult =await _userService.RemoveAsync(userResponseDto.Data);

        return CreateActionResult(userRemovalResult);
    }

    [HttpPost("[action]/{sessionId}")]
    public async Task<IActionResult> UpdateInformation(UserUpdateDto userDto,string sessionId)
    {
        var sessionWithUser = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);

        var updateResult = await _userService.UpdateUser(userDto,sessionWithUser);
        
        return  CreateActionResult(updateResult);
    }
  

}


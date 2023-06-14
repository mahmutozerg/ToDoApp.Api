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
        var user = await _userService.GetUserInfoByEmail(userLoginDto.Email);

        if (user == null)
            return CreateActionResult(CustomResponseNoDataDto.Fail(404,ResponseMessages.UserNotFound));

        var sessions = await _sessionService.Where(s=> s.UserId == user.Id).ToListAsync();
        await _sessionService.RemoveSessions(sessions);
        
        await _userService.RemoveAsync(user);

        return CreateActionResult(CustomResponseNoDataDto.Success(200));
    }

    [HttpPost("[action]/{sessionId}")]
    public async Task<IActionResult> UpdateInformation(UserUpdateDto userDto,string sessionId)
    {
        var sessionWithUser = await _sessionService.GetSessionWithUserBySessionIdAsync(sessionId);

        var updateResult = await _userService.UpdateUser(userDto,sessionWithUser);

        if (!string.IsNullOrEmpty(updateResult))
            return CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound, ResponseMessages.UserNotFound));
        
        return  CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK)); 

    }
  

}


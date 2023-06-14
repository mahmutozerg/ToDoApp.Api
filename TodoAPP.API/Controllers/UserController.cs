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
    private readonly IAuthenticationService _authenticationService;
    
    public UserController(IUserService userService, ISessionService sessionService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _sessionService = sessionService;
        _authenticationService = authenticationService;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        IActionResult actionResult;
        var registeredSession = await _authenticationService.Register(userDto);

        if (string.IsNullOrEmpty(registeredSession))
        {
            actionResult = CreateActionResult(CustomResponseNoDataDto.Fail(404, ResponseMessages.UserAlreadyExist));
            return actionResult;
        }
        
        actionResult = CreateActionResult(CustomResponseDto<SessionGuiddto>.Success(StatusCodes.Status200OK, new SessionGuiddto()
        {
            SessionGUID = registeredSession
        }));
        
        return actionResult;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        
        IActionResult actionResult;

        var session = await _authenticationService.Login(
            new UserDto()
            {
                Email = userDto.Email,
                Password = userDto.Password
            }
        );

        if (session == null)
        {
            actionResult = CreateActionResult(CustomResponseNoDataDto.Fail(404, ResponseMessages.UserNotFound));
            return actionResult;
        }
        
        actionResult= CreateActionResult(CustomResponseDto<SessionGuiddto>.Success(StatusCodes.Status200OK, new SessionGuiddto()
        {
            SessionGUID = session.SessionGUID
        }));
        
        return actionResult;    
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(SessionGuiddto sessionDto)
    {
        IActionResult actionResult = CreateActionResult(await _authenticationService.Logout(sessionDto));

        return actionResult;
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> Remove (UserLoginDto userLoginDto)
    {
        var user = await _userService.GetUserInfoByEmail(userLoginDto.Email);

        if (user == null)
            return CreateActionResult(CustomResponseNoDataDto.Fail(404,ResponseMessages.UserNotFound));

        var sessions = await _sessionService.Where(s=> s.UserId == user.Id).ToListAsync();
        await _sessionService.RemoveSessions(sessions);
        
        await _userService.Remove(user);

        return CreateActionResult(CustomResponseNoDataDto.Success(200));
    }

    [HttpPost("[action]/{sessionId}")]
    public async Task<IActionResult> UpdateInformation(UserUpdateDto userDto,string sessionId)
    {
        IActionResult actionResult;
        var sessionWithUser = await _sessionService.GetSessionWithUserBySessionId(sessionId);

        var updateResult = await _userService.UpdateUser(userDto,sessionWithUser);

        if (!string.IsNullOrEmpty(updateResult))
        {
            actionResult =  CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound, ResponseMessages.UserNotFound));
            return actionResult;
        }
        
        actionResult = CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK));
        return actionResult;

    }
  

}


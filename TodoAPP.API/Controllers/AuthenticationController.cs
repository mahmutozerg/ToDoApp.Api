using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Services;
namespace TodoAPP.API.Controllers;

public class AuthenticationController:CustomBaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterAsync(UserDto userDto)
    {
        var registeredSession = await _authenticationService.Register(userDto);

        if (string.IsNullOrEmpty(registeredSession))
            return CreateActionResult(CustomResponseNoDataDto.Fail(404, ResponseMessages.UserAlreadyExist));
        
         return CreateActionResult(CustomResponseDto<SessionGuiddto>.Success(StatusCodes.Status200OK, new SessionGuiddto()
        {
            SessionGUID = registeredSession
        }));
        
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        
        var session = await _authenticationService.Login(
            new UserDto()
            {
                Email = userDto.Email,
                Password = userDto.Password
            }
        );

        if (session == null)
            return  CreateActionResult(CustomResponseNoDataDto.Fail(404, ResponseMessages.UserNotFound));
        
        
        return CreateActionResult(CustomResponseDto<SessionGuiddto>.Success(StatusCodes.Status200OK, new SessionGuiddto()
        {
            SessionGUID = session.SessionGUID
        }));
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(SessionGuiddto sessionDto)
    {
        await _authenticationService.Logout(sessionDto);
        return CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK));

    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TodoAPP.Core.DTOs;
using TodoAPP.Core.Models;
using TodoAPP.Core.Services;
using TodoAPP.Service.Services;

namespace TodoAPP.API.Controllers;

public class UserController:CustomBaseController
{
    private readonly IUserInfoService _userInfoService;
    private readonly IUserInfoDetailedService _userInfoDetailedService;


    public UserController(IUserInfoService userInfoService, IMapper mapper, IUserInfoDetailedService userInfoDetailedService)
    {
        _userInfoService = userInfoService;
        _userInfoDetailedService = userInfoDetailedService;
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        IActionResult actionResult;
        UserInfoDetailed newUserDetails = await _userInfoDetailedService.CreateInitial();
        var errorMessage = await _userInfoService.Register(userDto,newUserDetails);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status409Conflict, errorMessage));
            return actionResult;
        }
        actionResult = CreateActionResult(CustomResponseDto<UserInfoDetailed>.Succes(StatusCodes.Status201Created,newUserDetails));
        return actionResult;

    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        IActionResult actionResult;
        var mappedUserDto = new UserDto()
        {
            Email = userDto.Email,
            Password = userDto.Password

        };
        var user = await _userInfoService.GetUserInfoWithDetailsByEmail(mappedUserDto.Email);

        if (user != null)
        {
            var loginErrorMessage = await _userInfoService.Login(mappedUserDto);
            if (string.IsNullOrEmpty(loginErrorMessage))
            {
                
                _userInfoDetailedService.UpdateOnLogin(user.UserInfoDetailed);
                await _userInfoService.Update(user);

                // Create a new cookie with the specified name and value

                var sessionResponse = new UserGUIDDTO()
                {
                    SessionGUID = user.UserInfoDetailed.SessionGUID
                };
                actionResult= CreateActionResult(CustomResponseDto<UserGUIDDTO>.Succes(StatusCodes.Status200OK,sessionResponse));
                return actionResult;
            }
            actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,$"User already logged in session id {user.UserInfoDetailed.SessionGUID}" ));
            return actionResult;
        }
        
        actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,$"Check your credentials {userDto.Email}:{userDto.Password}" ));
        return actionResult;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(UserGUIDDTO userDto)
    {
        IActionResult actionResult;
        var user = await _userInfoDetailedService.Where(x => x.SessionGUID == userDto.SessionGUID).FirstOrDefaultAsync();
        if (user == null || string.IsNullOrEmpty(user.SessionGUID))
        {
            actionResult=  CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,$"User already logged out or user does not exist check session id: {userDto.SessionGUID}" ));
            return actionResult;
        }

        user.IsUserActive = false;
        user.SessionGUID = string.Empty;
        await _userInfoDetailedService.Update(user);
        
        actionResult = CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK));
        return actionResult;

    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Remove (UserLoginDto userDto)
    {
        IActionResult actionResult;
        var mappedUserDto = new UserDto()
        {
            Email = userDto.Email,
            Password = userDto.Password
        };
        var user = await _userInfoService.GetUserInfoWithDetailsByEmail(mappedUserDto.Email);
        
        if (user != null)
        {
            var guidDto = new UserGUIDDTO()
            {
                SessionGUID = user.UserInfoDetailed.SessionGUID
            };
            
            await Logout(guidDto);
            
            var loginErrorMessage = await _userInfoService.Login(mappedUserDto);
            if (string.IsNullOrEmpty(loginErrorMessage))
            {                
                var  userInfoWithDetails =   await _userInfoService.GetUserInfoWithDetailsByEmail(mappedUserDto.Email);
                await _userInfoService.Remove(userDto.Email, userInfoWithDetails);
                
                actionResult= CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK ));
                return actionResult;
            }
            
        }
        actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,$"Check your credentials {userDto.Email}:{userDto.Password}" ));
        return actionResult;

    }
}


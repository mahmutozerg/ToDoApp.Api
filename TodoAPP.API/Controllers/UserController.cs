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
        var userExistErrorMessage = await _userInfoService.Register(userDto,newUserDetails);

        if (!string.IsNullOrEmpty(userExistErrorMessage))
        {
            actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status409Conflict, userExistErrorMessage));
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
        var userInfoWithDetails = await _userInfoService.GetUserInfoWithDetailsByEmail(mappedUserDto.Email);

        if (userInfoWithDetails != null)
        {
            var loginErrorMessage = await _userInfoService.Login(mappedUserDto);
            if (string.IsNullOrEmpty(loginErrorMessage))
            {
                
                _userInfoDetailedService.UpdateOnLogin(userInfoWithDetails.UserInfoDetailed);
                await _userInfoService.Update(userInfoWithDetails);

                // Create a new cookie with the specified name and value

                var sessionResponse = new UserGUIDDTO()
                {
                    SessionGUID = userInfoWithDetails.UserInfoDetailed.SessionGUID
                };
                actionResult= CreateActionResult(CustomResponseDto<UserGUIDDTO>.Succes(StatusCodes.Status200OK,sessionResponse));
                return actionResult;
            }
            actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,$"User already logged in session id {userInfoWithDetails.UserInfoDetailed.SessionGUID}" ));
            return actionResult;
        }
        
        actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,$"Check your credentials {userDto.Email}:{userDto.Password}" ));
        return actionResult;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(UserGUIDDTO userDto)
    {
        IActionResult actionResult;
        var userInfoWithDetails = await _userInfoDetailedService.Where(x => x.SessionGUID == userDto.SessionGUID).FirstOrDefaultAsync();
       
        if (userInfoWithDetails == null || string.IsNullOrEmpty(userInfoWithDetails.SessionGUID))
        {
            actionResult=  CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,$"User already logged out or user does not exist check session id: {userDto.SessionGUID}" ));
            return actionResult;
        }

        userInfoWithDetails.IsUserActive = false;
        userInfoWithDetails.SessionGUID = string.Empty;
        await _userInfoDetailedService.Update(userInfoWithDetails);
        
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
        var userInfoWithDetails = await _userInfoService.GetUserInfoWithDetailsByEmail(mappedUserDto.Email);
        
        if (userInfoWithDetails != null)
        {
            var guidDto = new UserGUIDDTO()
            {
                SessionGUID = userInfoWithDetails.UserInfoDetailed.SessionGUID
            };
            
            await Logout(guidDto);
            
            var loginErrorMessage = await _userInfoService.Login(mappedUserDto);
            if (string.IsNullOrEmpty(loginErrorMessage))
            {                
                await _userInfoService.Remove(userDto.Email, userInfoWithDetails);
                
                actionResult= CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK ));
                return actionResult;
            }
            
        }
        actionResult= CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status401Unauthorized,$"Check your credentials {userDto.Email}:{userDto.Password}" ));
        return actionResult;
    }

    [HttpPost("[action]/{sessionId}")]
    public async Task<IActionResult> UpdateInformation(UserDto userDto,string sessionId)
    {
        IActionResult actionResult;

        var isUserExist = await _userInfoService.CheckUserExist(userDto); 
        var userInfoWithDetails = await _userInfoService.GetUserInfoWithDetailsBySessionId(sessionId);

        if (!string.IsNullOrEmpty(isUserExist) && userDto.UserName != userInfoWithDetails.UserName && userDto.Email != userInfoWithDetails.Email)
        {
            actionResult = CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status409Conflict,isUserExist ));
            return actionResult;
        }
        if (userInfoWithDetails == null)
        {
            actionResult = CreateActionResult(CustomResponseNoDataDto.Fail(StatusCodes.Status404NotFound,$"User does not exist check session id: {sessionId}" ));
            return actionResult;
        }
        userInfoWithDetails.UserName = userDto.UserName;
        userInfoWithDetails.Email = userDto.Email;
        userInfoWithDetails.PasswordHash = _userInfoService.CalculateHash(userDto.Password,userInfoWithDetails.PasswordSalt);
        await _userInfoService.Update(userInfoWithDetails);
        
        actionResult = actionResult = CreateActionResult(CustomResponseNoDataDto.Success(StatusCodes.Status200OK ));
        return actionResult;

    }

}


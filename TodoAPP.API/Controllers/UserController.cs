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

    [HttpGet]
    public async Task<IEnumerable<UserInfo>> asa()
    {
        return await _userInfoService.GetAllUserInfoWithDetails();
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserDto userDto)
    { 
        UserInfoDetailed newUserDetails = await _userInfoDetailedService.CreateInitial();
        var errorMessage = await _userInfoService.Register(userDto,newUserDetails);

        if (string.IsNullOrEmpty(errorMessage))
        {
            return CreateActionResult(CustomResponseDto<UserInfoDetailed>.Succes(StatusCodes.Status201Created,newUserDetails));
        }
        
        return CreateActionResult(CustomResponseDto<UserLoginDto>.Fail(StatusCodes.Status409Conflict, errorMessage));
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        var mappedUserDto = new UserDto()
        {
            Email = userDto.Email,
            Password = userDto.Password

        };
        var errorMessage = await _userInfoService.CheckUserExist(mappedUserDto);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            var loginInformationMessage = await _userInfoService.Login(mappedUserDto);
            if (string.IsNullOrEmpty(loginInformationMessage))
            {
                var  userInfoWithDetails =   await _userInfoService.GetUserInfoWithDetailsByEmail(mappedUserDto.Email);
                
                _userInfoDetailedService.UpdateOnLogin(userInfoWithDetails.UserInfoDetailed);
                await _userInfoService.Update(userInfoWithDetails);

                // Create a new cookie with the specified name and value

                var sessionResponse = new UserGUIDDTO()
                {
                    SessionGUID = userInfoWithDetails.UserInfoDetailed.SessionGUID
                };
                return CreateActionResult(CustomResponseDto<UserGUIDDTO>.Succes(StatusCodes.Status200OK,sessionResponse));
            }
        }
        
        
        return CreateActionResult(CustomResponseDto<UserLoginDto>.Fail(StatusCodes.Status401Unauthorized,$"Check your credentials {userDto.Email}:{userDto.Password}" ));
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Logout(UserGUIDDTO userDto)
    {
        var user = await _userInfoDetailedService.Where(x => x.SessionGUID == userDto.SessionGUID).FirstOrDefaultAsync();
        if (user == null || string.IsNullOrEmpty(user.SessionGUID))
            return CreateActionResult(CustomResponseDto<UserLoginDto>.Fail(StatusCodes.Status404NotFound,$"Be sure that you are logged in" ));

        user.IsUserActive = false;
        user.SessionGUID = "";
        await _userInfoDetailedService.Update(user);
        return CreateActionResult(CustomResponseDto<UserGUIDDTO>.Succes(StatusCodes.Status200OK));


    }


}


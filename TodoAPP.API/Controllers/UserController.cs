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
        var newUserDetails = await _userInfoDetailedService.CreateInitial();
        var errorMessage = await _userInfoService.Register(userDto,newUserDetails);

        if (string.IsNullOrEmpty(errorMessage))
        {
            return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(StatusCodes.Status201Created));
        }
        
        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(StatusCodes.Status409Conflict, errorMessage));
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userDto)
    {
        var propUserDto = new UserDto()
        {
            UserName = userDto.UserNameOrEmail,
            Email = userDto.UserNameOrEmail,
            Password = userDto.Password

        };
        var errorMessage = await _userInfoService.CheckUserExist(propUserDto);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            var loginInformationMessage = await _userInfoService.Login(propUserDto);
            if (string.IsNullOrEmpty(loginInformationMessage))
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(StatusCodes.Status200OK));
            }
        }
        // bunu yapmamın sebebi olası bir saldırı yapan kişinin şifrenin mi yanlış olduğunu ya da name,email mi yanlış anlayamasın diye
        // status code olaral 401 i seçtim bu yüzden 404 döndürmektense 
        return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(StatusCodes.Status401Unauthorized,$"Check your credentials {userDto.UserNameOrEmail}:{userDto.Password}" ));
    }


}


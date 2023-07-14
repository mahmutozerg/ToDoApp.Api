using Microsoft.AspNetCore.Mvc;
using TodoAPP.Auth.Core.DTOs;
using TodoAPP.Auth.Core.Services;

namespace TodoApp.Auth.Controllers;


public class AuthController:CustomControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await _authenticationService.CreateTokenAsync(loginDto);


        return CreateActionResult(result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result = await _authenticationService.CreateTokenByClient(clientLoginDto);

        return CreateActionResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
    {
        var result = await _authenticationService.RevokeRefreshToken(refreshToken);
        return CreateActionResult(result);

    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(string refreshToken)
    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshToken);

        return CreateActionResult(result);
    }
}
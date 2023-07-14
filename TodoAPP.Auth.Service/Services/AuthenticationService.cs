using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Auth.Core.DTOs;
using TodoAPP.Auth.Core.Models;
using TodoAPP.Auth.Core.Repositories;
using TodoAPP.Auth.Core.Services;

namespace TodoAPP.Auth.Service.Services;

public class AuthenticationService:IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _refreshToken;


    public AuthenticationService(ITokenService tokenService, UserManager<User> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> refreshToken)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _refreshToken = refreshToken;
    }

    public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto is null)
            throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
            return Response<TokenDto>.Fail("Email or password is wrong", 400,true);

        if (! await _userManager.CheckPasswordAsync(user,loginDto.Password))
            return Response<TokenDto>.Fail("Email or password is wrong", 400,true);

        var token = _tokenService.CreateToken(user);

        var userRefreshToken = await _refreshToken.Where(r => r.UserId == user.Id).SingleOrDefaultAsync();

        if (userRefreshToken is null)
        {
            await _refreshToken.AddAsync(new()
                { UserId = user.Id, Token = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Token = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommitAsync();
        return Response<TokenDto>.Success(token,200);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var refreshTokenExist = await _refreshToken.Where(r => r.Token == refreshToken).FirstOrDefaultAsync();

        if (refreshTokenExist is null)
            return Response<TokenDto>.Fail("Refresh token does not exist", 404,true);

        var user = await _userManager.FindByIdAsync(refreshTokenExist.UserId);
        if (user is null)
            return Response<TokenDto>.Fail("User does not exist", 404,true);

        var token = _tokenService.CreateToken(user);
        refreshTokenExist.Token = token.RefreshToken;
        refreshTokenExist.Expiration = token.RefreshTokenExpiration;

        await _unitOfWork.CommitAsync();
        return Response<TokenDto>.Success(token, 200);

        
    }

    public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var refreshTokenExist = await _refreshToken.Where(r => r.Token == refreshToken).FirstOrDefaultAsync();
        if (refreshTokenExist is null)
            return Response<NoDataDto>.Fail("Refresh token does not exist", 404,true);
        
        _refreshToken.Remove(refreshTokenExist);
        await _unitOfWork.CommitAsync();


        return Response<NoDataDto>.Success(200);

    }

    public Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        throw new NotImplementedException();
    }
}
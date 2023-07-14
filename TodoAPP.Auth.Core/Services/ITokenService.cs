using TodoAPP.Auth.Core.Configurations;
using TodoAPP.Auth.Core.DTOs;
using TodoAPP.Auth.Core.Models;

namespace TodoAPP.Auth.Core.Services;

public interface ITokenService
{
    TokenDto CreateToken(User user);
    ClientTokenDto CreateTokenByClient(Client client);

}
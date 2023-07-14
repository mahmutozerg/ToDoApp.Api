namespace TodoAPP.Auth.Core.DTOs;

public class TokenDto
{
    public string AccesToken { get; set; }
    public DateTime AccesTokenExpiration { get; set; }
    
    
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
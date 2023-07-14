namespace TodoAPP.Auth.Core.DTOs;

public class ClientTokenDto
{
    public string AccesToken { get; set; }
    public DateTime AccesTokenExpiration { get; set; }
}
namespace TodoAPP.Auth.Service.Configurations;

public class AppTokenOptions
{
    public List<String> Audience { get; set; }
    public string Issuer { get; set; }
    public int AccesTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }
    public string SecurityKey { get; set; }
}
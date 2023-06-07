namespace TodoAPP.Core.DTOs;

public class CustomResponseNoDataDto
{
    public int StatusCode { get; set; }
    public string[] Errors { get; set; }
    
    public static CustomResponseNoDataDto Response(int statusCode,string errors)
    {
        return new CustomResponseNoDataDto { StatusCode = statusCode,Errors = errors.Split("\n")};
    }


}
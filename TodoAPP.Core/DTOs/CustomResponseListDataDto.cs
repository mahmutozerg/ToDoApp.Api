namespace TodoAPP.Core.DTOs;

public class CustomResponseListDataDto<T>

{
    public List<T>? Data { get; set; }
    public int StatusCode { get; set; }
    
    public static CustomResponseListDataDto<T> Success(int statusCode,List<T> data)
    {
        return new CustomResponseListDataDto<T> {Data = data, StatusCode = statusCode};
    }    


}
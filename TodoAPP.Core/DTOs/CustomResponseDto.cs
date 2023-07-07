using System.Text.Json.Serialization;

namespace TodoAPP.Core.DTOs;

public class CustomResponseDto<T>
{
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; }


    public static CustomResponseDto<T> Success(T data,int statusCode)
    {
        return new CustomResponseDto<T> {Data = data, StatusCode = statusCode,Errors = new List<string>()};
    }

    public static CustomResponseDto<T> Success(int statusCode)
    {
        return new CustomResponseDto<T> {StatusCode = statusCode,Errors = new List<string>()};
    }
    public static CustomResponseDto<T> Fail(List<string> errors,int statusCode)
    {
        return new CustomResponseDto<T> {Errors = errors, StatusCode = statusCode};
    }
    
    
    public static CustomResponseDto<T> Fail(string error,int statusCode)
    {
        return new CustomResponseDto<T> {Errors = new List<string>{error}, StatusCode = statusCode};
    }
}
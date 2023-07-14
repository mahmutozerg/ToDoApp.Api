using Microsoft.AspNetCore.Mvc;
using TodoAPP.Auth.Core.DTOs;

namespace TodoApp.Auth.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CustomControllerBase:ControllerBase 
{
    [NonAction]
    public IActionResult CreateActionResult<T>(Response<T> res) where T : class
    {

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }


}
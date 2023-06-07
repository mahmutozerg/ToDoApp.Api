using Microsoft.AspNetCore.Mvc;
using TodoAPP.Core.DTOs;

namespace TodoAPP.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomBaseController:ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T>(CustomResponseDto<T> res)
    {

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }
    [NonAction]

    public IActionResult CreateActionResult<T>(CustomResponseListDataDto<T> res)
    {


        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }    
    [NonAction]

    public IActionResult CreateActionResult(CustomResponseNoDataDto res)
    {
        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }    
}
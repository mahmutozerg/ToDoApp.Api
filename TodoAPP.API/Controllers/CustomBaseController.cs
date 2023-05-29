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
        if (res.StatusCode == 204)
        {
            return new ObjectResult(null)
            {
                StatusCode = res.StatusCode
            };
        }

        return new ObjectResult(res)
        {
            StatusCode = res.StatusCode
        };

    }
}
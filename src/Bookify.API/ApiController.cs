using System.Runtime.InteropServices.JavaScript;
using Bookify.Domain.Entities.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API;
[ApiController]
public class ApiController : ControllerBase
{
    [NonAction]
    public IActionResult ProblemDetails(Error error)
    {
        return Problem(
            type: "DomainErros",
            detail: error.Name,
            statusCode: error.StatusCode,
            title: error.Code.ToString()
            );
    }
}
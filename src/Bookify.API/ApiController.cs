using Bookify.API.Hypermedia;
using Bookify.Domain.Entities.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Asp.Versioning;
using System;

namespace Bookify.API;

[ApiController]
[Produces(MediaTypes.HateoasJson, MediaTypeNames.Application.Json)]
public class ApiController : ControllerBase
{
    [NonAction]
    public IActionResult ProblemDetails(Error error)
    {
        return Problem(
            type: "DomainErrors",
            detail: error.Name,
            statusCode: error.StatusCode,
            title: error.Code.ToString()
        );
    }

    protected ApiVersion GetRequestedApiVersion()
    {
        return HttpContext.GetRequestedApiVersion() ?? new ApiVersion(1, 0);
    }

    protected bool ShouldGenerateLinks()
    {
        var mediaType = HttpContext.Request.Headers.Accept.ToString();
        return mediaType.Contains(MediaTypes.HateoasJson);
    }
}
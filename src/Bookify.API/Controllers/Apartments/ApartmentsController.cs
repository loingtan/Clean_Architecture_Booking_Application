using Asp.Versioning;
using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers.Apartments;

[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/apartments")]
public class ApartmentsController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        DateOnly startDate, 
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(startDate, endDate);

        var result = await sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
    [HttpPost]
    public async Task<IActionResult> CreateApartment(
        CreateApartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateApartmentCommand(
            request.Name,
            request.Description,
            request.Price,
            request.Rooms,
            request.Beds,
            request.Bathrooms,
            request.Address,
            request.City,
            request.Country,
            request.Latitude,
            request.Longitude);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? ProblemDetails(result.Error) : CreatedAtAction(nameof(GetApartment), new { id = result.Value }, result.Value);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApartment(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetApartmentQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : ProblemDetails(result.Error);
    }
    [HttpPatch("{id}")]
}

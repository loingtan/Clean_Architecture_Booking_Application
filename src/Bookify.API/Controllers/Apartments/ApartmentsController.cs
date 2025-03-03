using Asp.Versioning;
using Bookify.Application.Apartments.CreateApartment;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Application.Apartments.UpdateApartment;
using Bookify.Application.Apartments.GetApartment;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        var command = request.Adapt<CreateApartmentCommand>();
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
    public async Task<IActionResult> UpdateApartment(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateApartmentRequest> patchDocument,
        CancellationToken cancellationToken)
    {

        var getQuery = new GetApartmentQuery(id);
        var getResult = await sender.Send(getQuery, cancellationToken);

        if (getResult.IsFailure)
            return ProblemDetails(getResult.Error);

        var updateRequest = new UpdateApartmentRequest();

        patchDocument.ApplyTo(updateRequest);
        
        if (!TryValidateModel(updateRequest))
            return BadRequest(ModelState);
        var command = updateRequest.Adapt<UpdateApartmentCommand>() with { Id = id };
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : ProblemDetails(result.Error);
    }
}


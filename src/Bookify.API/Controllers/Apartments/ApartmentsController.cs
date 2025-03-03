using Asp.Versioning;
using Bookify.Application.Apartments.CreateApartment;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Application.Apartments.UpdateApartment;
using Bookify.Application.Apartments.GetApartment;
using Bookify.Application.Common.Interfaces;
using Bookify.Application.Common.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Bookify.API.Controllers.Apartments;

[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/apartments")]
public class ApartmentsController(ISender sender, IDataShaper<Application.Apartments.SearchApartments.ApartmentResponse> dataShaper) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        [FromQuery] ApartmentParameters parameters,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(
            parameters.StartDate ?? DateOnly.FromDateTime(DateTime.Today),
            parameters.EndDate ?? DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            parameters.PageNumber,
            parameters.PageSize,
            parameters.Country,
            parameters.City);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        // Apply data shaping if fields parameter is provided
        if (!string.IsNullOrEmpty(parameters.Fields))
        {
            var shapedData = dataShaper.ShapeData(result.Value.Items, parameters.Fields);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(new
                {
                    result.Value.TotalCount,
                    result.Value.PageSize,
                    result.Value.CurrentPage,
                    result.Value.TotalPages,
                    result.Value.HasNext,
                    result.Value.HasPrevious
                }));

            return Ok(shapedData);
        }

        // Standard pagination response
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(new
            {
                result.Value.TotalCount,
                result.Value.PageSize,
                result.Value.CurrentPage,
                result.Value.TotalPages,
                result.Value.HasNext,
                result.Value.HasPrevious
            }));

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


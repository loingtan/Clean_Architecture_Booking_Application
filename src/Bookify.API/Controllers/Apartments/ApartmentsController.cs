using Asp.Versioning;
using Bookify.API.Hypermedia;
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
using System.Linq;

namespace Bookify.API.Controllers.Apartments;

[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/apartments")]
public class ApartmentsController : ApiController
{
    private readonly ISender _sender;
    private readonly IDataShaper<Bookify.Application.Apartments.SearchApartments.ApartmentResponse> _dataShaper;
    private readonly ILinkGenerator _linkGenerator;

    public ApartmentsController(
        ISender sender,
        IDataShaper<Bookify.Application.Apartments.SearchApartments.ApartmentResponse> dataShaper,
        ILinkGenerator linkGenerator)
    {
        _sender = sender;
        _dataShaper = dataShaper;
        _linkGenerator = linkGenerator;
    }

    [HttpGet(Name = "GetApartments")]
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

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        var paginationMetadata = new
        {
            result.Value.TotalCount,
            result.Value.PageSize,
            result.Value.CurrentPage,
            result.Value.TotalPages,
            result.Value.HasNext,
            result.Value.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        // Apply data shaping if fields parameter is provided
        if (!string.IsNullOrEmpty(parameters.Fields))
        {
            var shapedData = _dataShaper.ShapeData(result.Value.Items, parameters.Fields);
            return Ok(shapedData);
        }

        // Check if HATEOAS links should be included
        if (ShouldGenerateLinks())
        {
            var apartmentsWithLinks = result.Value.Items.Select(apartment =>
            {
                var wrapper = new EntityResponseWrapper<Bookify.Application.Apartments.SearchApartments.ApartmentResponse>(apartment)
                {
                    Links = _linkGenerator.GenerateLinks("Apartment", new { id = apartment.Id }, GetRequestedApiVersion())
                };
                return wrapper;
            });

            var pagination = new Pagination
            {
                TotalCount = result.Value.TotalCount,
                PageSize = result.Value.PageSize,
                CurrentPage = result.Value.CurrentPage,
                TotalPages = result.Value.TotalPages,
                HasNext = result.Value.HasNext,
                HasPrevious = result.Value.HasPrevious
            };

            var wrapper = new CollectionResponseWrapper<EntityResponseWrapper<Bookify.Application.Apartments.SearchApartments.ApartmentResponse>>(
                apartmentsWithLinks,
                pagination)
            {
                Links = _linkGenerator.GenerateCollectionLinks(
                    "Apartment",
                    new
                    {
                        parameters.StartDate,
                        parameters.EndDate,
                        parameters.PageNumber,
                        parameters.PageSize,
                        parameters.Country,
                        parameters.City
                    },
                    result.Value.HasNext,
                    result.Value.HasPrevious,
                    GetRequestedApiVersion())
            };

            return Ok(wrapper);
        }

        // Standard response without HATEOAS
        return Ok(result.Value);
    }

    [HttpGet("{id}", Name = "GetApartment")]
    public async Task<IActionResult> GetApartment(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetApartmentQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        if (ShouldGenerateLinks())
        {
            var wrapper = new EntityResponseWrapper<Application.Apartments.GetApartment.ApartmentResponse>(result.Value);
            wrapper.Links = _linkGenerator.GenerateLinks("Apartment", new { id }, GetRequestedApiVersion());
            return Ok(wrapper);
        }

        return Ok(result.Value);
    }

    [HttpPost(Name = "CreateApartment")]
    public async Task<IActionResult> CreateApartment(
        CreateApartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateApartmentCommand>();
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        if (ShouldGenerateLinks())
        {
            var wrapper = new EntityResponseWrapper<object>(new { result.Value })
            {
                Links = _linkGenerator.GenerateLinks("Apartment", new { id = result.Value }, GetRequestedApiVersion())
            };
            return CreatedAtAction(nameof(GetApartment), new { id = result.Value }, wrapper);
        }

        return CreatedAtAction(nameof(GetApartment), new { id = result.Value }, result.Value);
    }

    [HttpPatch("{id}", Name = "UpdateApartment")]
    public async Task<IActionResult> UpdateApartment(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateApartmentRequest> patchDocument,
        CancellationToken cancellationToken)
    {
        var getQuery = new GetApartmentQuery(id);
        var getResult = await _sender.Send(getQuery, cancellationToken);

        if (getResult.IsFailure)
            return ProblemDetails(getResult.Error);

        var updateRequest = new UpdateApartmentRequest();
        patchDocument.ApplyTo(updateRequest);

        if (!TryValidateModel(updateRequest))
            return BadRequest(ModelState);

        var command = updateRequest.Adapt<UpdateApartmentCommand>() with { Id = id };
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        if (ShouldGenerateLinks())
        {
            var wrapper = new EntityResponseWrapper<object>(new { id })
            {
                Links = _linkGenerator.GenerateLinks("Apartment", new { id }, GetRequestedApiVersion())
            };
            return Ok(wrapper);
        }

        return NoContent();
    }
}
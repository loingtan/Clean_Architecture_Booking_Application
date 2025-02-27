﻿using Asp.Versioning;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers.Bookings;
[Authorize]

[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/bookings")]
public class BookingsController : ApiController
{
    private readonly ISender _sender;

    public BookingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : ProblemDetails(result.Error);  
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsFailure ? ProblemDetails(result.Error) : CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}

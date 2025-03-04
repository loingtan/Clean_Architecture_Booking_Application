using Asp.Versioning;
using Bookify.API.Hypermedia;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bookify.API.Controllers.Bookings;

[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/bookings")]
public class BookingsController : ApiController
{
    private readonly ISender _sender;
    private readonly ILinkGenerator _linkGenerator;

    public BookingsController(ISender sender, ILinkGenerator linkGenerator)
    {
        _sender = sender;
        _linkGenerator = linkGenerator;
    }

    [HttpGet("{id}", Name = "GetBooking")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        if (ShouldGenerateLinks())
        {
            var wrapper = new EntityResponseWrapper<BookingResponse>(result.Value);
            wrapper.Links = _linkGenerator.GenerateLinks("Booking", new { id }, GetRequestedApiVersion());
            return Ok(wrapper);
        }

        return Ok(result.Value);
    }

    [HttpPost(Name = "CreateBooking")]
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

        if (result.IsFailure)
            return ProblemDetails(result.Error);

        if (ShouldGenerateLinks())
        {
            var wrapper = new EntityResponseWrapper<string>(result.Value.ToString());
            wrapper.Links = _linkGenerator.GenerateLinks("Booking", new { id = result.Value }, GetRequestedApiVersion());
            return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, wrapper);
        }

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}
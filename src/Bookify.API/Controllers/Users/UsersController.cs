﻿using Asp.Versioning;
using Bookify.Application.Users.GetAllUsers;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Application.Users.LogInUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.API.Controllers.Users;


[ApiVersion(ApiVersions.V1)]
[ApiVersion(ApiVersions.V2)]
[Route("api/v{version:apiVersion}/users")]
public class UsersController(ISender sender) : ApiController
{
    [HttpGet("me")]
    [MapToApiVersion(ApiVersions.V1)]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetLoggedInUserV1(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();
        var result = await sender.Send(query, cancellationToken);
        return Ok(result.Value);

    }

    [HttpGet("me")]
    [MapToApiVersion(ApiVersions.V2)]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetLoggedInUserV2(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();

        var result = await sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? ProblemDetails(result.Error) : Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LogInUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(
            request.Email,
            request.Password);

        var result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? ProblemDetails(result.Error) : Ok(result.Value);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery()
        {
            PageNumber = request.pageNumber,
            PageSize = request.pageSize,
        };
        var result = await sender.Send(query, cancellationToken);
        return result.IsFailure ? ProblemDetails(result.Error) : Ok(result.Value);
    }
}

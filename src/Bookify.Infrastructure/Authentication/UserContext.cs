﻿using Bookify.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Bookify.Infrastructure.Authentication;
internal sealed class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    public Guid UserId =>
        contextAccessor
            .HttpContext?.User
            .GetUserId() ?? throw new ApplicationException("User context is unavailable");

    public string IdentityId =>
        contextAccessor
            .HttpContext?.User
            .GetIdentityId() ?? throw new ApplicationException("User context is unavailable");    
}

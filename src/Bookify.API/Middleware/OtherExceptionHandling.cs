using Microsoft.AspNetCore.Diagnostics;

namespace Bookify.API.Middleware;

public class OtherExceptionHandling : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
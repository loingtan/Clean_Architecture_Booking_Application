using Bookify.Api.Middleware;
using Bookify.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Bookify.API.Middleware;

public class GlobalExceptionHandling(IProblemDetailsService problemDetailsService,  ILogger<GlobalExceptionHandling> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException)
        {
            return false;
        }
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        var exceptionDetails = GetExceptionDetails(exception);
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
                Extensions =
                {
                    ["errors"] = exceptionDetails.Errors ?? new object()
                }
            },
            Exception = exception
        });
        
    }
    private static ExceptionHandlingMiddleware.ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionHandlingMiddleware.ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validation error",
                "One or more validation errors has occurred",
                validationException.Errors),            
            _ => new ExceptionHandlingMiddleware.ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Server error",
                "An unexpected error has occurred",
                null)
        };
    }

    internal record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object> Errors);
}
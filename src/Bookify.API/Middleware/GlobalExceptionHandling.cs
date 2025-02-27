using Bookify.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Bookify.API.Middleware
{
    public class GlobalExceptionHandling(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandling> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionDetails = GetExceptionDetails(exception);
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
            var problemDetailsContext = new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Status = exceptionDetails.Status,
                    Type = exceptionDetails.Type,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Detail ?? "An unexpected error has occurred.",
                    Extensions =
                    {
                        ["traceId"] = httpContext.TraceIdentifier,
                        ["instance"] = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                        ["errors"] = exceptionDetails.Errors ?? new object()
                    }
                },
                Exception = exception
            };
            
            return await problemDetailsService.TryWriteAsync(problemDetailsContext);
        }

        /// <summary>
        /// Maps the thrown exception to details for constructing the ProblemDetails response.
        /// </summary>
        private static ExceptionDetails GetExceptionDetails(Exception exception) =>
            exception switch
            {
    
                ValidationException validationException => new ExceptionDetails(
                    Status: StatusCodes.Status400BadRequest,
                    Type: "ValidationFailure",
                    Title: "Validation Error",
                    Detail: "One or more validation errors occurred.",
                    Errors: validationException.Errors),
                NotFoundException notFoundException => new ExceptionDetails(
                    Status: StatusCodes.Status404NotFound,
                    Type: "NotFound",
                    Title: "Resource Not Found",
                    Detail: notFoundException.Message,
                    Errors: null),
                UnauthorizedException unauthorizedException => new ExceptionDetails(
                    Status: StatusCodes.Status401Unauthorized,
                    Type: "Unauthorized",
                    Title: "Unauthorized",
                    Detail: unauthorizedException.Message,
                    Errors: null),
                ForbiddenException forbiddenException => new ExceptionDetails(
                    Status: StatusCodes.Status403Forbidden,
                    Type: "Forbidden",
                    Title: "Forbidden",
                    Detail: forbiddenException.Message,
                    Errors: null),
                ConflictException conflictException => new ExceptionDetails(
                    Status: StatusCodes.Status409Conflict,
                    Type: "Conflict",
                    Title: "Conflict",
                    Detail: conflictException.Message,
                    Errors: null),

                _ => new ExceptionDetails(
                    Status: StatusCodes.Status500InternalServerError,
                    Type: "ServerError",
                    Title: "Server Error",
                    Detail: "An unexpected error has occurred.",
                    Errors: null)
            };

        /// <summary>
        /// Record to hold exception details for the ProblemDetails response.
        /// </summary>
        internal record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            string Detail,
            IEnumerable<object> Errors);
    }
}

namespace Bookify.Domain.Entities.Abstractions;

public record Error(string Code, string Name, int StatusCode = 400)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    public static readonly Error NotFound = new("Error.NotFound", "The specified entity was not found", 404);
    public static readonly Error InvalidValue = new("Error.InvalidValue", "The provided value is invalid");
    public static readonly Error InvalidState = new("Error.InvalidState", "The entity is in an invalid state");
    public static readonly Error NotAuthorized = new("Error.NotAuthorized", "The user is not authorized to perform this operation", 403);
    public static readonly Error NotAuthenticated = new("Error.NotAuthenticated", "The user is not authenticated", 401);
    public static readonly Error NotSupported = new("Error.NotSupported", "The operation is not supported", 405);
    public static readonly Error NotAllowed = new("Error.NotAllowed", "The operation is not allowed", 403);
    public static readonly Error NotAvailable = new("Error.NotAvailable", "The requested resource is not available", 503);
    public static readonly Error NotUnique = new("Error.NotUnique", "The provided value is not unique", 409);
    public static readonly Error Conflict = new("Error.Conflict", "The operation conflicts with an existing entity", 409);
    public static readonly Error Timeout = new("Error.Timeout", "The operation timed out", 408);
    public static readonly Error RateLimited = new("Error.RateLimited", "Too many requests. Try again later.", 429);
    public static readonly Error BadRequest = new("Error.BadRequest", "The request is malformed or contains invalid data", 400);
    public static readonly Error InternalServerError = new("Error.InternalServerError", "An unexpected error occurred", 500);
    public static readonly Error DependencyFailed = new("Error.DependencyFailed", "A dependent service or operation failed", 424);
    public static readonly Error PaymentRequired = new("Error.PaymentRequired", "Payment is required to complete this action", 402);
    public static readonly Error Forbidden = new("Error.Forbidden", "Access to this resource is forbidden", 403);
    public static readonly Error MethodNotAllowed = new("Error.MethodNotAllowed", "The HTTP method is not allowed for this endpoint", 405);
    public static readonly Error UnprocessableEntity = new("Error.UnprocessableEntity", "The request was well-formed but contains semantic errors", 422);
    public static readonly Error UnsupportedMediaType = new("Error.UnsupportedMediaType", "The media type of the request is not supported", 415);
    public static readonly Error Locked = new("Error.Locked", "The resource is locked and cannot be modified", 423);
    public static readonly Error PreconditionFailed = new("Error.PreconditionFailed", "A required precondition is missing or invalid", 412);
    public static readonly Error TooManyRequests = new("Error.TooManyRequests", "The client has sent too many requests in a short period", 429);
    public static readonly Error FailedDependency = new("Error.FailedDependency", "A dependent request failed", 424);
}

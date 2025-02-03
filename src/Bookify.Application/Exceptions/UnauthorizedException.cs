namespace Bookify.Application.Exceptions;

public sealed class UnauthorizedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}
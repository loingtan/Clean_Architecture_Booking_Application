namespace Bookify.Application.Exceptions;

public sealed class ForbiddenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ForbiddenException(string message)
        : base(message)
    {
    }
}
namespace Bookify.Application.Exceptions;

public sealed class ConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ConflictException(string message)
        : base(message)
    {
    }
}

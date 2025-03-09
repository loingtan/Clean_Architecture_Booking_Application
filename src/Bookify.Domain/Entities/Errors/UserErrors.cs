using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.User;
public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.NotFound",
        "The user with the specified identifier was not found");
}


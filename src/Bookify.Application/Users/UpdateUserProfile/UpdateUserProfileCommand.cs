using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.GetLoggedInUser;

namespace Bookify.Application.Users;

public sealed record UpdateUserProfileCommand(string FirstName,
    string LastName,
    string Password): ICommand<UserResponse>;

using Bookify.Application.Abstractions.Messaging;
namespace Bookify.Application.Users;

public sealed record UpdateUserProfileCommand(string FirstName,
    string LastName,
    string Password): ICommand;

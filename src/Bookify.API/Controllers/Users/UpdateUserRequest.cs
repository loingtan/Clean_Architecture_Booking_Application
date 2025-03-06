namespace Bookify.API.Controllers.Users
{
    public sealed record UpdateUserRequest(string FirstName,
    string LastName,
    string Password);
}

namespace Bookify.API.Controllers.Users;

public sealed record GetAllUsersRequest(
    int pageSize, int pageNumber);
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.GetLoggedInUser;

namespace Bookify.Application.Users.GetAllUsers;

public class GetAllUsersQuery : IQuery<List<UserResponse>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}

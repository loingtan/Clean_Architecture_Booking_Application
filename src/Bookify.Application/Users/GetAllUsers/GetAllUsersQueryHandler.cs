using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Domain.Entities.Abstractions;
using Dapper;

namespace Bookify.Application.Users.GetAllUsers;

public sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAllUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                       SELECT 
                           id AS Id,
                           first_name AS FirstName,
                           last_name AS LastName,
                           email AS Email
                       FROM users
                       ORDER BY id
                       LIMIT @pageSize OFFSET (@pageNumber - 1) * @pageSize;
                       """;

        var users = (await connection.QueryAsync<UserResponse>(
            sql,
            new
            {
                pageSize = query.PageSize,
                pageNumber = query.PageNumber,
            }
        )).ToList();

        return Result.Success(users);
    }

}

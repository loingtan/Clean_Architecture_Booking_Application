﻿using Bookify.Application.Users;
using Bookify.Domain.Entities.Users;

namespace Bookify.Infrastructure.Repositories;
internal sealed class UserRepository(ApplicationDbContext dbContext)
    : Repository<User, UserId>(dbContext), IUserRepository
{
    public override void Add(User user)
    {
        //This will tell EF Core that any roles present on our user object are already inside of the database and you don't
        //need to insert them again 
        foreach (var role in user.Roles)
        {
            DbContext.Attach(role);
        }

        DbContext.Add(user);
    }

    public async Task<User> Update(UserId id, User user, CancellationToken cancellationToken = default)
    {
        var existingUser = await GetByIdAsync(id, cancellationToken);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {user.Id} not found");
        }

        DbContext.Entry(existingUser).CurrentValues.SetValues(user);

        await DbContext.SaveChangesAsync(cancellationToken);
        return existingUser;

    }
}

﻿using Bookify.Domain.Entities.Abstractions;
using Bookify.Domain.Entities.Users.Events;
using Bookify.Domain.Entities.Users.ValueObjects;

namespace Bookify.Domain.Entities.Users;

public sealed class User :  AuditableEntity<UserId>
{
    private readonly List<Role> _roles = new();

    public User(UserId id, string firstName, string lastName, Email email) 
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public User() { }

    public string IdentityId { get; private set; } = string.Empty;
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public IReadOnlyCollection<Role> Roles => _roles;

    public static User Create(string firstName, string lastName, Email email)
    {
        var user = new User(UserId.New(), firstName, lastName, email);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        user._roles.Add(Role.Registered);

        return user;
    }
    public void Update(string firstName = null, string lastName = null)
    {
        if(firstName != null)
        {
            FirstName = firstName;
        }
        if(lastName != null)
        {
            LastName = lastName;
        }
    }

    public void SetIdentityId(string identityId) => IdentityId = identityId;
}

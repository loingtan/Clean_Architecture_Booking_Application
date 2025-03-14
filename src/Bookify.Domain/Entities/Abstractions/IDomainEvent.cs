﻿using MediatR;

namespace Bookify.Domain.Entities.Abstractions;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
};


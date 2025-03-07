﻿using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.Entities.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent
{ 
    public Guid Id => ReviewId.Value;
};
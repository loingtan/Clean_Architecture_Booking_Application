using Bookify.Domain.Entities.Abstractions;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
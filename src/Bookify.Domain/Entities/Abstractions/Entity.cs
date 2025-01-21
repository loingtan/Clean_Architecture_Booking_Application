using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Domain.Entities.Abstractions;

public abstract class Entity<TEntityId> : IEntity
{
    [NotMapped]
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(TEntityId id)
    {
        Id = id;
    }
    protected Entity() { }

    public TEntityId Id { get; init; }

    public void ClearDomainEvents() => _domainEvents.Clear();
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
}

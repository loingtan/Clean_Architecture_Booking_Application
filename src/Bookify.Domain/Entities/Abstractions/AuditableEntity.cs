namespace Bookify.Domain.Entities.Abstractions;

public class AuditableEntity<TEntityId> : Entity<TEntityId>
{
    protected AuditableEntity(TEntityId id) : base(id) { }
    protected AuditableEntity() { }
    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}
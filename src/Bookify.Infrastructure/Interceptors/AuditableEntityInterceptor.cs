using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bookify.Infrastructure.Interceptors;

public class AuditableEntityInterceptor(
    IUserContext user,
    TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (!entry.Entity.GetType().IsAssignableTo(typeof(AuditableEntity<>)) ||
                (entry.State is not (EntityState.Added or EntityState.Modified) &&
                 !entry.HasChangedOwnedEntities())) continue;
            var utcNow = dateTime.GetUtcNow();
            dynamic entity = entry.Entity; 

            if (entry.State == EntityState.Added)
            {
                entity.CreatedBy = user.UserId;
                entity.Created = utcNow;
            }

            entity.LastModifiedBy = user.UserId;
            entity.LastModified = utcNow;
        }
    }

}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
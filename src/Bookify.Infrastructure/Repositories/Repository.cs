using Bookify.Domain.Entities.Abstractions;
using Bookify.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Repositories;
internal abstract class Repository<TEntity, TEntityId>(ApplicationDbContext dbContext)
    where TEntity : AuditableEntity<TEntityId>
    where TEntityId : class
{
    protected readonly ApplicationDbContext DbContext = dbContext;

    public async Task<TEntity> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
    public IQueryable<TEntity> ApplySpecification(Specification<TEntity, TEntityId> specifications)
    {
        return SpecificationEvaluator.GetQuery(DbContext.Set<TEntity>(), specifications);
    }
    public virtual void Add(TEntity entity) => DbContext.Add(entity);
}

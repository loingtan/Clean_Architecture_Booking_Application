using System.Linq.Expressions;
using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Infrastructure.Specifications;

public abstract class Specification<TEntity, TEntityId>(Expression<Func<TEntity, bool>>? criteria)
    where TEntity : AuditableEntity<TEntityId>
    where TEntityId : class
{
    public bool IsSplitQuery { get; protected set; }

    public Expression<Func<TEntity, bool>>? Criteria { get; } = criteria;

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        IncludeExpressions.Add(includeExpression);

    protected void AddOrderBy(
        Expression<Func<TEntity, object>> orderByExpression) =>
        OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(
        Expression<Func<TEntity, object>> orderByDescendingExpression) =>
        OrderByDescendingExpression = orderByDescendingExpression;
}

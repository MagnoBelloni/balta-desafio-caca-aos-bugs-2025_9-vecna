using System.Linq.Expressions;

namespace BugStore.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>> predicate,
            int page,
            int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            CancellationToken cancellationToken);

        Task<T?> GetOneAsNoTrackingAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<T?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}

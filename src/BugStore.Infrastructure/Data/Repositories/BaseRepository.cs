using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BugStore.Infrastructure.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetOneAsNoTrackingAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>> filter,
            int page,
            int pageSize,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            CancellationToken cancellationToken)
        {
            IQueryable<T> query = _dbSet
                .AsExpandableEFCore()
                .AsNoTracking()
                .Where(filter);

            var totalCount = await query.CountAsync(cancellationToken);

            query = orderBy(query);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
            => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
            => await _dbSet
                .AsExpandableEFCore()
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => await _dbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

        public virtual async Task<T?> GetByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken)
            => await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await GetByIdAsync(id, cancellationToken);
            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

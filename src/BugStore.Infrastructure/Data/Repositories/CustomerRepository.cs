using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using BugStore.Domain.Responses.Reports;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Data.Repositories
{
    public class CustomerRepository(AppDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
    {
        public async Task<(IEnumerable<BestCustomerResponse> Items, int TotalCount)> GetPagedBestCustomers(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbSet
            .AsExpandableEFCore()
            .AsNoTracking()
            .Select(c => new BestCustomerResponse
            {
                CustomerName = c.Name,
                CustomerEmail = c.Email,
                TotalOrders = c.Orders.Count(),
                SpentAmount = c.Orders.Sum(o => o.TotalAmount)
            })
            .Where(x => x.TotalOrders > 0)
            .OrderByDescending(x => x.TotalOrders);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}

using BugStore.Domain.Dtos.Reports;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Interfaces.CacheRepositories
{
    public interface IOrderCacheRepository
    {
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task<RevenueByPeriodDto?> GetRevenueByPeriodAsync(int year, int month, CancellationToken cancellationToken);
    }
}

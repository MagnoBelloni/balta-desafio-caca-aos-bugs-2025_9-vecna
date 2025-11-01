using BugStore.Domain.Dtos.Reports;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Interfaces.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetOrderByIdAsNoTrackingWithIncludesAsync(Guid id, CancellationToken cancellationToken);
        Task<RevenueByPeriodDto?> GetRevenueByPeriodAsync(int year, int month, CancellationToken cancellationToken);
    }
}

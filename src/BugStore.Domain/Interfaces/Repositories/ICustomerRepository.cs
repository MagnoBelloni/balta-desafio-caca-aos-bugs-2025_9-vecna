using BugStore.Domain.Entities;
using BugStore.Domain.Responses.Reports;

namespace BugStore.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<(IEnumerable<BestCustomerResponse> Items, int TotalCount)> GetPagedBestCustomers(int page, int pageSize, CancellationToken cancellationToken);
    }
}

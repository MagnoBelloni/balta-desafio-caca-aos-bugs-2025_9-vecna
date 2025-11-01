using BugStore.Application.Reports.Requests;
using BugStore.Domain.Dtos;
using BugStore.Domain.Interfaces.Repositories;
using BugStore.Domain.Responses.Reports;
using MediatR;

namespace BugStore.Application.Reports.Handlers
{
    public class BestCustomerRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<BestCustomerRequest, PagedResponseDto<IEnumerable<BestCustomerResponse>>>
    {
        public async Task<PagedResponseDto<IEnumerable<BestCustomerResponse>>> Handle(BestCustomerRequest request, CancellationToken cancellationToken)
        {
            var (page, pageSize) = request.GetPageInfo();

            var (pagedBestCustomers, totalCount) = await customerRepository.GetPagedBestCustomers(
                page,
                pageSize,
                cancellationToken);

            return new PagedResponseDto<IEnumerable<BestCustomerResponse>>(pagedBestCustomers, page, pageSize, totalCount);
        }
    }
}

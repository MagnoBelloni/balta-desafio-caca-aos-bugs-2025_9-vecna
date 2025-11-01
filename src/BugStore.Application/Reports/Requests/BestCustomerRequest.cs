using BugStore.Domain.Dtos;
using BugStore.Domain.Responses.Reports;
using MediatR;

namespace BugStore.Application.Reports.Requests
{
    public class BestCustomerRequest : PagedRequestDto, IRequest<PagedResponseDto<IEnumerable<BestCustomerResponse>>>;
}

using BugStore.Application.Customers.Responses;
using BugStore.Domain.Dtos;
using MediatR;

namespace BugStore.Application.Customers.Requests;

public class GetCustomersRequest : PagedRequestDto, IRequest<PagedResponseDto<GetCustomersResponse>>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
using BugStore.Application.Customers.Responses;
using MediatR;

namespace BugStore.Application.Customers.Requests;

public class GetCustomerByIdRequest : IRequest<GetCustomerByIdResponse?>
{
    public Guid Id { get; set; }
}
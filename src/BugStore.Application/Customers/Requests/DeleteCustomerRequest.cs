using BugStore.Application.Customers.Responses;
using MediatR;

namespace BugStore.Application.Customers.Requests;

public class DeleteCustomerRequest : IRequest<DeleteCustomerResponse>
{
    public Guid Id { get; set; }
}
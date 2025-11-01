using BugStore.Application.Customers.Requests;
using BugStore.Application.Customers.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Customers.Handlers;

public class DeleteCustomerRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<DeleteCustomerRequest, DeleteCustomerResponse>
{
    public async Task<DeleteCustomerResponse> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        await customerRepository.DeleteAsync(request.Id, cancellationToken);
        return new DeleteCustomerResponse();
    }
}

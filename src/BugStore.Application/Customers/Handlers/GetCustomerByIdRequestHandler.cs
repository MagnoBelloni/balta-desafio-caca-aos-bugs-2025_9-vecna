using BugStore.Application.Customers.Requests;
using BugStore.Application.Customers.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Customers.Handlers;

public class GetCustomerByIdRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByIdRequest, GetCustomerByIdResponse?>
{
    public async Task<GetCustomerByIdResponse?> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (customer is null)
            return null;

        return GetCustomerByIdResponse.FromCustomer(customer);
    }
}

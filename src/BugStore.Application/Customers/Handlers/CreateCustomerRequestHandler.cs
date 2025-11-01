using BugStore.Application.Customers.Requests;
using BugStore.Application.Customers.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Customers.Handlers;

public class CreateCustomerRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customerByEmail = await customerRepository.GetOneAsNoTrackingAsync(x => x.Email == request.Email, cancellationToken);
        if (customerByEmail is not null)
        {
            throw new Exception("Já existe um customer com esse email");
        }

        var customer = request.ToCustomer();

        await customerRepository.AddAsync(customer, cancellationToken);

        return CreateCustomerResponse.FromCustomer(customer);
    }
}
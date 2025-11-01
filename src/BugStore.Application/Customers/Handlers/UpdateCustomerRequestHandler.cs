using BugStore.Application.Customers.Requests;
using BugStore.Application.Customers.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Customers.Handlers;

public class UpdateCustomerRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception("Customer não encontrado");

        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.BirthDate = request.BirthDate;

        await customerRepository.UpdateAsync(customer, cancellationToken);

        return UpdateCustomerResponse.FromCustomer(customer);
    }
}

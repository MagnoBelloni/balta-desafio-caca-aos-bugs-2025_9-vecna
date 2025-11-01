using BugStore.Application.Customers.Responses;
using BugStore.Domain.Entities;
using MediatR;

namespace BugStore.Application.Customers.Requests;

public class CreateCustomerRequest : IRequest<CreateCustomerResponse>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime BirthDate { get; set; }

    public Customer ToCustomer()
    {
        return new Customer
        {
            Name = Name,
            Email = Email,
            Phone = Phone,
            BirthDate = BirthDate
        };
    }
}
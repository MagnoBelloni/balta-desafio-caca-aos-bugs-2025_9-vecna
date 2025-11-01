using BugStore.Application.Customers.Responses;
using MediatR;

namespace BugStore.Application.Customers.Requests;

public class UpdateCustomerRequest : IRequest<UpdateCustomerResponse>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime BirthDate { get; set; }
}
using BugStore.Domain.Entities;

namespace BugStore.Application.Customers.Responses;

public class UpdateCustomerResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required DateTime BirthDate { get; set; }

    public static UpdateCustomerResponse FromCustomer(Customer customer)
    {
        return new UpdateCustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }
}
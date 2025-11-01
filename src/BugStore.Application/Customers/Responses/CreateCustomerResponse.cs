using BugStore.Domain.Entities;

namespace BugStore.Application.Customers.Responses;

public class CreateCustomerResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime BirthDate { get; set; }

    public static CreateCustomerResponse FromCustomer(Customer customer)
    {
        return new CreateCustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }
}
using BugStore.Domain.Entities;

namespace BugStore.Application.Customers.Responses;

public class GetCustomerByIdResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime BirthDate { get; set; }

    public static GetCustomerByIdResponse FromCustomer(Customer customer)
    {
        return new GetCustomerByIdResponse()
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }
}
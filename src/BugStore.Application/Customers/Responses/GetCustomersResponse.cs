using BugStore.Domain.Entities;

namespace BugStore.Application.Customers.Responses;

public class GetCustomersResponse
{
    public IEnumerable<GetCustomerByIdResponse> Customers { get; set; } = [];

    public static GetCustomersResponse FromCustomer(IEnumerable<Customer> customer)
    {
        return new GetCustomersResponse
        {
            Customers = customer.Select(GetCustomerByIdResponse.FromCustomer)
        };
    }
}
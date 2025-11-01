using BugStore.Domain.Entities;

namespace BugStore.Application.Orders.Responses;

public class GetOrderByIdResponse
{
    public Guid Id { get; set; }
    public Customer? Customer { get; private set; }
    public List<OrderLine> Lines { get; private set; } = [];
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static GetOrderByIdResponse FromOrder(Order order)
    {
        return new GetOrderByIdResponse
        {
            Id = order.Id,
            Customer = order.Customer,
            Lines = order.Lines,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}
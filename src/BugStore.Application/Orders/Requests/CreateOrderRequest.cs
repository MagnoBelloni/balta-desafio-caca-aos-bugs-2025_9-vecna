using BugStore.Application.Orders.Responses;
using MediatR;

namespace BugStore.Application.Orders.Requests;

public class CreateOrderRequest : IRequest<CreateOrderResponse>
{
    public Guid CustomerId { get; set; }
    public List<OrderLineRequest> Lines { get; set; } = [];
}

public class OrderLineRequest
{
    public int Quantity { get; set; }

    public Guid ProductId { get; set; }
}
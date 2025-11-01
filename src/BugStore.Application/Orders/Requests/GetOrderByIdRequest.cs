using BugStore.Application.Orders.Responses;
using MediatR;

namespace BugStore.Application.Orders.Requests;

public class GetOrderByIdRequest : IRequest<GetOrderByIdResponse?>
{
    public Guid Id { get; set; }
}
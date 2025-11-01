using BugStore.Application.Orders.Requests;
using BugStore.Application.Orders.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Orders.Handlers
{
    public class GetOrderByIdRequestHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse?>
    {
        public async Task<GetOrderByIdResponse?> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetOrderByIdAsNoTrackingWithIncludesAsync(request.Id, cancellationToken);
            if (order is null)
                return null;

            return GetOrderByIdResponse.FromOrder(order);
        }
    }
}

using BugStore.Domain.Entities;

namespace BugStore.Application.Orders.Responses
{
    public class GetOrderResponse
    {
        public IEnumerable<GetOrderByIdResponse> Orders { get; set; } = [];

        public static GetOrderResponse FromOrder(IEnumerable<Order> orders)
        {
            return new GetOrderResponse
            {
                Orders = orders.Select(GetOrderByIdResponse.FromOrder)
            };
        }
    }
}

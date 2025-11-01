namespace BugStore.Application.Orders.Responses;

public record CreateOrderResponse(Guid OrderId, decimal TotalAmount);
using BugStore.Application.Orders.Requests;
using MediatR;

namespace BugStore.Api.Controllers
{
    public static class OrdersController
    {
        public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/v1/orders");


            group.MapGet("/", async ([AsParameters] GetOrderRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetOrderByIdRequest { Id = id });
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });

            group.MapPost("/", async (CreateOrderRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Created($"/v1/orders/{result.OrderId}", result);
            });
        }
    }
}

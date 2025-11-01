using BugStore.Application.Customers.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Api.Controllers
{
    public static class CustomersController
    {
        public static void MapCustomersEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/v1/customers");

            group.MapGet("/", async ([AsParameters] GetCustomersRequest request, [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCustomerByIdRequest { Id = id });
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });

            group.MapPost("/", async (CreateCustomerRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Created($"/v1/customers/{result.Id}", result);
            });

            group.MapPut("/{id:guid}", async (Guid id, UpdateCustomerRequest request, IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteCustomerRequest { Id = id });
                return Results.NoContent();
            });
        }
    }
}

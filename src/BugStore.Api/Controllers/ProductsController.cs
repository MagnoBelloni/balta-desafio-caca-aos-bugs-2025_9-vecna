using BugStore.Application.Products.Requests;
using MediatR;

namespace BugStore.Api.Controllers
{
    public static class ProductsController
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/v1/products");

            group.MapGet("/", async ([AsParameters] GetProductRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductByIdRequest { Id = id });
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });

            group.MapPost("/", async (CreateProductRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Created($"/v1/products/{result.Id}", result);
            });

            group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, IMediator mediator) =>
            {
                request.Id = id;
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                await mediator.Send(new DeleteProductRequest { Id = id });
                return Results.NoContent();
            });
        }
    }
}

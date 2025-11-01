using BugStore.Application.Reports.Requests;
using MediatR;

namespace BugStore.Api.Controllers
{
    public static class ReportsController
    {
        public static void MapReportsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/v1/reports");


            group.MapGet("/customer", async ([AsParameters] BestCustomerRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return Results.Ok(result);
            });

            group.MapGet("/revenue", async ([AsParameters] RevenueByPeriodRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(request);
                return result is not null ? Results.Ok(result) : Results.NotFound();
            });
        }
    }
}

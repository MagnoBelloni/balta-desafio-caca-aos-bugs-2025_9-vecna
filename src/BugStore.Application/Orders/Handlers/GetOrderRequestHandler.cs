using BugStore.Application.Orders.Requests;
using BugStore.Application.Orders.Responses;
using BugStore.Domain.Dtos;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using LinqKit;
using MediatR;
using System.Linq.Expressions;

namespace BugStore.Application.Orders.Handlers;

public class GetOrderRequestHandler(IOrderRepository orderRepository) : IRequestHandler<GetOrderRequest, PagedResponseDto<GetOrderResponse>>
{
    private static IOrderedQueryable<Order> OrderBy(IQueryable<Order> q) => q.OrderByDescending(c => c.CreatedAt);

    public async Task<PagedResponseDto<GetOrderResponse>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
    {
        var filter = GetOrderFilter(request);

        var (page, pageSize) = request.GetPageInfo();

        var (orders, totalCount) = await orderRepository.GetPagedAsync(filter,
            page,
            pageSize,
            OrderBy,
            cancellationToken);

        return new PagedResponseDto<GetOrderResponse>(
            GetOrderResponse.FromOrder(orders),
            page,
            pageSize,
            totalCount);
    }

    private static Expression<Func<Order, bool>> GetOrderFilter(GetOrderRequest request)
    {
        var filter = PredicateBuilder.New<Order>(true);

        if (request.Id is not null && request.Id != Guid.Empty)
            filter = filter.And(x => x.Id == request.Id);

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
            filter = filter.And(x => x.Customer!.Name.Contains(request.CustomerName));

        if (!string.IsNullOrWhiteSpace(request.CustomerEmail))
            filter = filter.And(x => x.Customer!.Email.Contains(request.CustomerEmail));

        if (!string.IsNullOrWhiteSpace(request.CustomerPhone))
            filter = filter.And(x => x.Customer!.Phone.Contains(request.CustomerPhone));

        if (!string.IsNullOrWhiteSpace(request.ProductTitle))
            filter = filter.And(x => x.Lines!.Any(l => l.Product!.Title.Contains(request.ProductTitle)));

        if (!string.IsNullOrWhiteSpace(request.ProductDescription))
            filter = filter.And(x => x.Lines!.Any(l => l.Product!.Description.Contains(request.ProductDescription)));

        if (!string.IsNullOrWhiteSpace(request.ProductSlug))
            filter = filter.And(x => x.Lines!.Any(l => l.Product!.Slug.Contains(request.ProductSlug)));

        if (request.ProductPriceStart is not null)
            filter = filter.And(x => x.Lines!.Any(l => l.Product!.Price >= request.ProductPriceStart));

        if (request.ProductPriceEnd is not null)
            filter = filter.And(x => x.Lines!.Any(l => l.Product!.Price <= request.ProductPriceEnd));

        if (request.CreatedAtStart is not null)
            filter = filter.And(x => x.CreatedAt >= request.CreatedAtStart);

        if (request.CreatedAtEnd is not null)
            filter = filter.And(x => x.CreatedAt <= request.CreatedAtEnd);

        if (request.UpdatedAtStart is not null)
            filter = filter.And(x => x.UpdatedAt >= request.UpdatedAtStart);

        if (request.UpdatedAtEnd is not null)
            filter = filter.And(x => x.UpdatedAt <= request.UpdatedAtEnd);

        return filter;
    }
}

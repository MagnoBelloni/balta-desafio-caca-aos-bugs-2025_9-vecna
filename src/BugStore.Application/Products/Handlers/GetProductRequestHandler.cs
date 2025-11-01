using BugStore.Application.Products.Requests;
using BugStore.Application.Products.Responses;
using BugStore.Domain.Dtos;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using LinqKit;
using MediatR;
using System.Linq.Expressions;

namespace BugStore.Application.Products.Handlers;

public class GetProductRequestHandler(IProductRepository productRepository) : IRequestHandler<GetProductRequest, PagedResponseDto<GetProductResponse>>
{
    private static IOrderedQueryable<Product> OrderBy(IQueryable<Product> q) => q.OrderByDescending(c => c.CreatedAt);

    public async Task<PagedResponseDto<GetProductResponse>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var filter = GetProductFilter(request);

        var (page, pageSize) = request.GetPageInfo();

        var (products, totalCount) = await productRepository.GetPagedAsync(filter,
            page,
            pageSize,
            OrderBy,
            cancellationToken);

        return new PagedResponseDto<GetProductResponse>(GetProductResponse.FromProduct(products), page, pageSize, totalCount);
    }

    public static Expression<Func<Product, bool>> GetProductFilter(GetProductRequest request)
    {
        var filter = PredicateBuilder.New<Product>(true);

        if (!string.IsNullOrWhiteSpace(request.Title))
            filter = filter.And(x => x.Title.Contains(request.Title));

        if (!string.IsNullOrWhiteSpace(request.Description))
            filter = filter.And(x => x.Description.Contains(request.Description));

        if (!string.IsNullOrWhiteSpace(request.Slug))
            filter = filter.And(x => x.Slug.Contains(request.Slug));

        if (request.Price is not null)
            filter = filter.And(x => x.Price == request.Price);

        return filter;
    }
}

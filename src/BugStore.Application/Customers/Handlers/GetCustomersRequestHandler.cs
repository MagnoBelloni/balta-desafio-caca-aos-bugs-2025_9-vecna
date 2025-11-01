using BugStore.Application.Customers.Requests;
using BugStore.Application.Customers.Responses;
using BugStore.Domain.Dtos;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using LinqKit;
using MediatR;
using System.Linq.Expressions;

namespace BugStore.Application.Customers.Handlers;

public class GetCustomersRequestHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomersRequest, PagedResponseDto<GetCustomersResponse>>
{
    private static IOrderedQueryable<Customer> OrderBy(IQueryable<Customer> q) => q.OrderByDescending(c => c.CreatedAt);

    public async Task<PagedResponseDto<GetCustomersResponse>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        var filter = GetCustomerFilter(request);

        var (page, pageSize) = request.GetPageInfo();

        var (customers, totalCount) = await customerRepository.GetPagedAsync(filter,
            page,
            pageSize,
            OrderBy,
            cancellationToken);

        return new PagedResponseDto<GetCustomersResponse>(
            GetCustomersResponse.FromCustomer(customers),
            page,
            pageSize,
            totalCount);
    }

    public static Expression<Func<Customer, bool>> GetCustomerFilter(GetCustomersRequest request)
    {
        var filter = PredicateBuilder.New<Customer>(true);

        if (!string.IsNullOrWhiteSpace(request.Name))
            filter = filter.And(x => x.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.Phone))
            filter = filter.And(x => x.Phone.Contains(request.Phone));

        if (!string.IsNullOrWhiteSpace(request.Email))
            filter = filter.And(x => x.Email.Contains(request.Email));

        return filter;
    }
}

using BugStore.Application.Reports.Requests;
using BugStore.Application.Reports.Responses;
using BugStore.Domain.Helpers;
using BugStore.Domain.Interfaces.CacheRepositories;
using MediatR;

namespace BugStore.Application.Reports.Handlers
{
    public class RevenueByPeriodRequestHandler(IOrderCacheRepository orderRepository) : IRequestHandler<RevenueByPeriodRequest, RevenueByPeriodResponse>
    {
        public async Task<RevenueByPeriodResponse> Handle(RevenueByPeriodRequest request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                throw new Exception("Requisição inválida");

            var revenue = await orderRepository.GetRevenueByPeriodAsync(request.Year, request.Month, cancellationToken);

            return new RevenueByPeriodResponse
            {
                TotalRevenue = revenue?.TotalRevenue ?? 0,
                TotalOrders = revenue?.TotalOrders ?? 0,
                Year = request.Year,
                Month = CultureInfoHelper.GetMonthNameByInteger(request.Month)
            };
        }
    }
}

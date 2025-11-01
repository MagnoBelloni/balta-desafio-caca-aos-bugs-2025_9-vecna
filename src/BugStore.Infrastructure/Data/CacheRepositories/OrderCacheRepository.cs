using BugStore.Domain.Constants;
using BugStore.Domain.Dtos.Reports;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.CacheRepositories;
using BugStore.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace BugStore.Infrastructure.Data.CacheRepositories
{
    public class OrderCacheRepository(IOrderRepository orderRepository, IMemoryCache memoryCache) : IOrderCacheRepository
    {
        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await orderRepository.AddAsync(order, cancellationToken);
            var formatedCacheKey = string.Format(CacheConstants.REVENUE_CACHE_KEY, order.CreatedAt.Year, order.CreatedAt.Month);
            memoryCache.Remove(formatedCacheKey);
        }

        public async Task<RevenueByPeriodDto?> GetRevenueByPeriodAsync(int year, int month, CancellationToken cancellationToken)
        {
            var formatedCacheKey = string.Format(CacheConstants.REVENUE_CACHE_KEY, year, month);
            if (!memoryCache.TryGetValue(formatedCacheKey, out RevenueByPeriodDto? revenueByPeriodDto))
            {
                revenueByPeriodDto = await orderRepository.GetRevenueByPeriodAsync(year, month, cancellationToken);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                memoryCache.Set(formatedCacheKey, revenueByPeriodDto, cacheOptions);
            }

            return revenueByPeriodDto;
        }
    }
}

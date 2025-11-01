using AutoFixture;
using BugStore.Domain.Dtos.Reports;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using BugStore.Infrastructure.Data.CacheRepositories;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace BugStore.Infrastructure.Tests.Data.CacheRepositories
{
    public class OrderCacheRepositoryTests
    {
        private readonly Fixture fixture;
        private readonly Mock<IOrderRepository> orderRepository;
        private readonly Mock<IMemoryCache> memoryCache;
        private readonly Mock<ICacheEntry> cacheEntry;
        private readonly OrderCacheRepository orderCacheRepository;

        public OrderCacheRepositoryTests()
        {
            fixture = new Fixture();
            orderRepository = new Mock<IOrderRepository>();
            cacheEntry = new Mock<ICacheEntry>();
            memoryCache = new Mock<IMemoryCache>();

            memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntry.Object);

            orderCacheRepository = new OrderCacheRepository(orderRepository.Object, memoryCache.Object);
        }

        [Fact]
        public async Task AddAsync_Should_AddOrderAndRemoveCache()
        {
            var order = fixture.Create<Order>();

            await orderCacheRepository.AddAsync(order, CancellationToken.None);

            orderRepository.Verify(x => x.AddAsync(It.IsAny<Order>(), CancellationToken.None), Times.Once());
            memoryCache.Verify(x => x.Remove(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetRevenueByPeriodAsync_Should_GetFromCacheWhenValueIsPresent()
        {
            object revenueByPeriodDto = fixture.Create<RevenueByPeriodDto>();

            memoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out revenueByPeriodDto!))
                .Returns(true);

            var result = await orderCacheRepository.GetRevenueByPeriodAsync(10, 2025, CancellationToken.None);

            result.Should().NotBeNull();
            orderRepository.Verify(x => x.GetRevenueByPeriodAsync(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task GetRevenueByPeriodAsync_Should_GetFromRepositoryWhenValueIsNOTPresent()
        {
            object revenueByPeriodDtoCache = null!;

            memoryCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out revenueByPeriodDtoCache!))
                .Returns(false);

            var revenueByPeriodDto = fixture.Create<RevenueByPeriodDto>();

            orderRepository.Setup(x => x.GetRevenueByPeriodAsync(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(revenueByPeriodDto);

            var result = await orderCacheRepository.GetRevenueByPeriodAsync(10, 2025, CancellationToken.None);

            result.Should().NotBeNull();
            orderRepository.Verify(x => x.GetRevenueByPeriodAsync(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None), Times.Once);
        }
    }
}

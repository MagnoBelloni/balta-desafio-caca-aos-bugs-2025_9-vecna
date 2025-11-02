using AutoFixture;
using BugStore.Application.Reports.Handlers;
using BugStore.Application.Reports.Requests;
using BugStore.Application.Reports.Responses;
using BugStore.Domain.Dtos.Reports;
using BugStore.Domain.Exceptions;
using BugStore.Domain.Helpers;
using BugStore.Domain.Interfaces.CacheRepositories;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Reports
{
    public class RevenueByPeriodRequestHandlerTests
    {
        private readonly Fixture fixture;
        private readonly Mock<IOrderCacheRepository> orderCacheRepository;
        private readonly RevenueByPeriodRequestHandler handler;

        public RevenueByPeriodRequestHandlerTests()
        {
            fixture = new Fixture();
            orderCacheRepository = new Mock<IOrderCacheRepository>();

            handler = new RevenueByPeriodRequestHandler(orderCacheRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnRevenueByPeriodResponse()
        {
            var expectedMonth = 12;
            var expectedYear = 2023;

            var request = fixture.Build<RevenueByPeriodRequest>()
                .With(x => x.Month, expectedMonth)
                .With(x => x.Year, expectedYear)
                .Create();

            var expectedRevenue = fixture.Build<RevenueByPeriodResponse>()
                .With(x => x.Month, CultureInfoHelper.GetMonthNameByInteger(expectedMonth))
                .With(x => x.Year, expectedYear)
                .Create();

            orderCacheRepository
                .Setup(repo => repo.GetRevenueByPeriodAsync(request.Year, request.Month, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RevenueByPeriodDto
                {
                    TotalRevenue = expectedRevenue.TotalRevenue,
                    TotalOrders = expectedRevenue.TotalOrders
                });

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedRevenue.TotalRevenue, result.TotalRevenue);
            Assert.Equal(expectedRevenue.TotalOrders, result.TotalOrders);
            Assert.Equal(request.Year, result.Year);
            Assert.Equal(expectedRevenue.Month, result.Month);
        }

        [Fact]
        public async Task Handle_Should_ThorwException_When_YearIsGreatherThanCurrentYear()
        {
            var expectedMonth = 12;

            var year = DateTime.Now.Year + 1;

            var request = fixture.Build<RevenueByPeriodRequest>()
              .With(x => x.Month, expectedMonth)
              .With(x => x.Year, year)
              .Create();

            var result = await Assert.ThrowsAsync<CustomAppException>(() => handler.Handle(request, CancellationToken.None));
            result.Message.Should().Be("O ano não pode ser maior que o atual");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public async Task Handle_Should_ThorwExceptionWhenMonthIsInvalid(int month)
        {
            var expectedYear = 2023;

            var request = fixture.Build<RevenueByPeriodRequest>()
              .With(x => x.Month, month)
              .With(x => x.Year, expectedYear)
              .Create();

            var result = await Assert.ThrowsAsync<CustomAppException>(() => handler.Handle(request, CancellationToken.None));
            result.Message.Should().Be("O mês precisa ser entre 1 e 12");
        }
    }
}

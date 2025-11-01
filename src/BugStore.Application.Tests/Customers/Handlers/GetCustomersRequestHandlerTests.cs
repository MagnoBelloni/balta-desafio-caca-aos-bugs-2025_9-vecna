using AutoFixture;
using BugStore.Application.Customers.Handlers;
using BugStore.Application.Customers.Requests;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace BugStore.Application.Tests.Customers.Handlers
{
    public class GetCustomersRequestHandlerTests
    {
        private readonly Fixture fixture;
        private readonly Mock<ICustomerRepository> customerRepository;
        private readonly GetCustomersRequestHandler handler;

        public GetCustomersRequestHandlerTests()
        {
            fixture = new Fixture();
            customerRepository = new Mock<ICustomerRepository>();
            handler = new GetCustomersRequestHandler(customerRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmpty_WhenNoCustomerFound()
        {
            var expectedCount = 3;

            customerRepository.Setup(x => x.GetPagedAsync(
               It.IsAny<Expression<Func<Customer, bool>>>(),
               It.IsAny<int>(),
               It.IsAny<int>(),
               It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>(),
               CancellationToken.None))
               .ReturnsAsync(([], expectedCount));

            var request = fixture.Create<GetCustomersRequest>();

            var result = await handler.Handle(request, CancellationToken.None);

            result.TotalCount.Should().Be(expectedCount);
            result.Response.Should().NotBeNull();
            result.Response.Customers.Should().NotBeNull();
            result.Response.Customers.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_ReturnResults_WhenSuccess()
        {
            var expectedCount = 3;
            var customer = fixture.CreateMany<Customer>(expectedCount);

            customerRepository.Setup(x => x.GetPagedAsync(
                It.IsAny<Expression<Func<Customer, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Customer>, IOrderedQueryable<Customer>>>(),
                CancellationToken.None))
                .ReturnsAsync((customer, expectedCount));

            var request = fixture.Create<GetCustomersRequest>();

            var result = await handler.Handle(request, CancellationToken.None);

            result.TotalCount.Should().Be(expectedCount);
            result.Response.Should().NotBeNull();
            result.Response.Customers.Should().NotBeNull();
            result.Response.Customers.Should().HaveCount(expectedCount);
        }
    }
}

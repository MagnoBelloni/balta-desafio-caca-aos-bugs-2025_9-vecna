using AutoFixture;
using BugStore.Application.Customers.Handlers;
using BugStore.Application.Customers.Requests;
using BugStore.Application.Customers.Responses;
using BugStore.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Customers.Handlers;

public class DeleteCustomerRequestHandlerTests
{
    private readonly Fixture fixture;
    private readonly Mock<ICustomerRepository> customerRepository;
    private readonly DeleteCustomerRequestHandler handler;

    public DeleteCustomerRequestHandlerTests()
    {
        fixture = new Fixture();
        customerRepository = new Mock<ICustomerRepository>();

        handler = new DeleteCustomerRequestHandler(customerRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_NotThrowException()
    {
        var request = fixture.Create<DeleteCustomerRequest>();

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().BeOfType<DeleteCustomerResponse>();
    }
}

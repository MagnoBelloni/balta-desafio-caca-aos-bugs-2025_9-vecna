using AutoFixture;
using BugStore.Application.Customers.Handlers;
using BugStore.Application.Customers.Requests;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Customers.Handlers;

public class GetCustomerByIdRequestHandlerTests
{
    private readonly Fixture fixture;
    private readonly Mock<ICustomerRepository> customerRepository;
    private readonly GetCustomerByIdRequestHandler handler;

    public GetCustomerByIdRequestHandlerTests()
    {
        fixture = new Fixture();
        customerRepository = new Mock<ICustomerRepository>();
        handler = new GetCustomerByIdRequestHandler(customerRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_WhenNotFound()
    {
        customerRepository.Setup(x => x.GetByIdAsNoTrackingAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync((Customer)null!);

        var request = fixture.Create<GetCustomerByIdRequest>();

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnResult_WhenSuccess()
    {
        var customer = fixture.Create<Customer>();

        customerRepository.Setup(x => x.GetByIdAsNoTrackingAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(customer);

        var request = fixture.Create<GetCustomerByIdRequest>();

        var result = await handler.Handle(request, CancellationToken.None);

        result!.Id.Should().Be(customer.Id);
        result.Name.Should().Be(customer.Name);
        result.Email.Should().Be(customer.Email);
        result.Phone.Should().Be(customer.Phone);
        result.BirthDate.Should().Be(customer.BirthDate);
    }
}

using AutoFixture;
using BugStore.Application.Customers.Handlers;
using BugStore.Application.Customers.Requests;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace BugStore.Application.Tests.Customers.Handlers;

public class UpdateCustomerRequestHandlerTests
{
    private readonly Fixture fixture;
    private readonly Mock<ICustomerRepository> customerRepository;
    private readonly UpdateCustomerRequestHandler handler;

    public UpdateCustomerRequestHandlerTests()
    {
        fixture = new Fixture();
        customerRepository = new Mock<ICustomerRepository>();
        handler = new UpdateCustomerRequestHandler(customerRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenEmailAlreadyExists()
    {
        var customer = fixture.Create<Customer>();

        customerRepository.Setup(x => x.GetOneAsNoTrackingAsync(It.IsAny<Expression<Func<Customer, bool>>>(), CancellationToken.None))
            .ReturnsAsync((Customer)null!);

        var request = fixture.Create<UpdateCustomerRequest>();

        var result = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

        result.Message.Should().Be("Customer não encontrado");
    }

    [Fact]
    public async Task Handle_Should_ReturnResult_WhenSuccess()
    {
        var customer = fixture.Create<Customer>();

        customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(customer);

        var request = fixture.Create<UpdateCustomerRequest>();

        var result = await handler.Handle(request, CancellationToken.None);

        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
        result.Phone.Should().Be(request.Phone);
        result.BirthDate.Should().Be(request.BirthDate);
    }
}

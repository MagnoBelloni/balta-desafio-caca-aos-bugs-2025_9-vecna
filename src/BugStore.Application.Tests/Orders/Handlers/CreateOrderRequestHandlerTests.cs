using AutoFixture;
using BugStore.Application.Orders.Handlers;
using BugStore.Application.Orders.Requests;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.CacheRepositories;
using BugStore.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace BugStore.Application.Tests.Orders.Handlers;

public class CreateOrderRequestHandlerTests
{
    private readonly Fixture fixture;
    private readonly Mock<ICustomerRepository> customerRepository;
    private readonly Mock<IProductRepository> productRepository;
    private readonly Mock<IOrderCacheRepository> orderRepository;
    private readonly CreateOrderRequestHandler handler;

    public CreateOrderRequestHandlerTests()
    {
        fixture = new Fixture();
        customerRepository = new Mock<ICustomerRepository>();
        productRepository = new Mock<IProductRepository>();
        orderRepository = new Mock<IOrderCacheRepository>();

        handler = new CreateOrderRequestHandler(
            customerRepository.Object,
            productRepository.Object,
            orderRepository.Object);
    }

    [Fact]
    public async Task Handle_When_CustomerDoestNOTExists_Should_ThrowException()
    {
        var request = fixture.Create<CreateOrderRequest>();

        var result = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

        result.Message.Should().Be("Customer não encontrado");
    }

    [Fact]
    public async Task Handle_When_ProductDoestNOTExists_Should_ThrowException()
    {
        var customer = fixture.Create<Customer>();
        customerRepository.Setup(x => x.GetByIdAsNoTrackingAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(customer);

        var productFoundId = Guid.NewGuid();
        var productFound = fixture.Build<Product>()
            .With(x => x.Id, productFoundId)
            .Create();

        productRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None))
            .ReturnsAsync([productFound]);

        var lineFound = fixture.Build<OrderLineRequest>()
            .With(x => x.ProductId, productFoundId)
            .Create();

        var productNotFoundId = Guid.NewGuid();
        var lineNotFound = fixture.Build<OrderLineRequest>()
            .With(x => x.ProductId, productNotFoundId)
            .Create();

        var request = fixture.Build<CreateOrderRequest>()
            .With(x => x.Lines, [lineFound, lineNotFound])
            .Create();

        var result = await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));

        result.Message.Should().Be($"Produto {productNotFoundId} não encontrado");
    }

    [Fact]
    public async Task Handle_When_CreateOrderWithSucess_Should_ReturnOrderTotalAmount()
    {
        var customer = fixture.Create<Customer>();
        customerRepository.Setup(x => x.GetByIdAsNoTrackingAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(customer);

        var product1Id = Guid.NewGuid();
        var product1 = fixture.Build<Product>()
            .With(x => x.Id, product1Id)
            .Create();

        var product2Id = Guid.NewGuid();
        var product2 = fixture.Build<Product>()
            .With(x => x.Id, product2Id)
            .Create();

        IEnumerable<Product> products = [product1, product2];

        productRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>>(), CancellationToken.None))
            .ReturnsAsync(products);

        var line1 = fixture.Build<OrderLineRequest>()
            .With(x => x.ProductId, product1Id)
            .Create();

        var line2 = fixture.Build<OrderLineRequest>()
            .With(x => x.ProductId, product2Id)
            .Create();

        var request = fixture.Build<CreateOrderRequest>()
                    .With(x => x.Lines, [line1, line2])
                    .Create();

        var expectedTotalAmount = GetExpectedTotalAmount(products, request);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalAmount.Should().Be(expectedTotalAmount);
    }

    private static decimal GetExpectedTotalAmount(IEnumerable<Product> products, CreateOrderRequest request)
    {
        decimal total = 0;
        foreach (var item in products)
        {
            var linePrice = request.Lines.First(x => item.Id == x.ProductId).Quantity;
            total += item.Price * linePrice;
        }

        return total;
    }
}

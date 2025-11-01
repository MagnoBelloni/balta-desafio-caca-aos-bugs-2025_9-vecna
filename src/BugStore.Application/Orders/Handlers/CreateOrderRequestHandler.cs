using BugStore.Application.Orders.Requests;
using BugStore.Application.Orders.Responses;
using BugStore.Domain.Entities;
using BugStore.Domain.Interfaces.CacheRepositories;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Orders.Handlers;

public class CreateOrderRequestHandler(ICustomerRepository customerRepository, IProductRepository productRepository, IOrderCacheRepository orderRepository) : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsNoTrackingAsync(request.CustomerId, cancellationToken)
            ?? throw new Exception("Customer não encontrado");

        var requestProductsIds = request.Lines.Select(x => x.ProductId);
        var products = await productRepository.GetAllAsync(x => requestProductsIds.Contains(x.Id), cancellationToken);
        CheckIfAllProductsExists(products, requestProductsIds);

        var lines = BuildOrderLines(products, request);
        var order = new Order(request.CustomerId, lines);

        await orderRepository.AddAsync(order, cancellationToken);

        return new CreateOrderResponse(order.Id, order.TotalAmount);
    }

    private static void CheckIfAllProductsExists(IEnumerable<Product> products, IEnumerable<Guid> requestProductsIds)
    {
        foreach (var productId in requestProductsIds)
        {
            if (!products.Any(x => x.Id == productId))
                throw new Exception($"Produto {productId} não encontrado");
        }
    }

    private static List<OrderLine> BuildOrderLines(IEnumerable<Product> products, CreateOrderRequest request)
    {
        List<OrderLine> orderLines = [];
        foreach (var product in products)
        {
            var requestLine = request.Lines.First(x => x.ProductId == product.Id);
            orderLines.Add(new OrderLine(requestLine.Quantity, product.Id, product));
        }

        return orderLines;
    }
}

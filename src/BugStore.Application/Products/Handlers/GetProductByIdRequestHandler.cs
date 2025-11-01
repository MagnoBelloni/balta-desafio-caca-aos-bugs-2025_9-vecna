using BugStore.Application.Products.Requests;
using BugStore.Application.Products.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Products.Handlers;

public class GetProductByIdRequestHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse?>
{
    public async Task<GetProductByIdResponse?> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsNoTrackingAsync(request.Id, cancellationToken);

        if (product is null)
            return null;

        return GetProductByIdResponse.FromProduct(product);
    }
}

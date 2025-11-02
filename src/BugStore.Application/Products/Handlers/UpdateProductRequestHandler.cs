using BugStore.Application.Products.Requests;
using BugStore.Application.Products.Responses;
using BugStore.Domain.Exceptions;
using BugStore.Domain.Helpers;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Products.Handlers;

public class UpdateProductRequestHandler(IProductRepository productRepository) : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new CustomAppException("Produto não encontrado");

        product.Title = request.Title;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Slug = ProductHelper.GetSlug(request.Title);

        await productRepository.UpdateAsync(product, cancellationToken);

        return UpdateProductResponse.FromProduct(product);
    }
}

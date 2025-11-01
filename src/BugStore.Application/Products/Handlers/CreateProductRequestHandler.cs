using BugStore.Application.Products.Requests;
using BugStore.Application.Products.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Products.Handlers
{
    public class CreateProductRequestHandler(IProductRepository productRepository) : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productByTitle = await productRepository.GetOneAsNoTrackingAsync(x => x.Title == request.Title, cancellationToken);
            if (productByTitle is not null)
            {
                throw new Exception("Já existe um product com esse titulo");
            }

            var product = request.ToProduct();

            await productRepository.AddAsync(product, cancellationToken);

            return new CreateProductResponse(product.Id);
        }
    }
}

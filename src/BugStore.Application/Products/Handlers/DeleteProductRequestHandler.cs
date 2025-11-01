using BugStore.Application.Products.Requests;
using BugStore.Application.Products.Responses;
using BugStore.Domain.Interfaces.Repositories;
using MediatR;

namespace BugStore.Application.Products.Handlers
{
    public class DeleteProductRequestHandler(IProductRepository productRepository) : IRequestHandler<DeleteProductRequest, DeleteProductResponse>
    {
        public async Task<DeleteProductResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            await productRepository.DeleteAsync(request.Id, cancellationToken);
            return new DeleteProductResponse();
        }
    }
}

using BugStore.Application.Products.Responses;
using MediatR;

namespace BugStore.Application.Products.Requests;

public class DeleteProductRequest : IRequest<DeleteProductResponse>
{
    public Guid Id { get; set; }
}
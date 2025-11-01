using BugStore.Application.Products.Responses;
using MediatR;

namespace BugStore.Application.Products.Requests;

public class UpdateProductRequest : IRequest<UpdateProductResponse>
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
}
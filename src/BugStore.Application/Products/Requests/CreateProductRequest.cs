using BugStore.Application.Products.Responses;
using BugStore.Domain.Entities;
using BugStore.Domain.Helpers;
using MediatR;

namespace BugStore.Application.Products.Requests;

public class CreateProductRequest : IRequest<CreateProductResponse>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }

    public Product ToProduct()
    {
        return new Product
        {
            Title = Title,
            Description = Description,
            Price = Price,
            Slug = ProductHelper.GetSlug(Title)
        };
    }
}
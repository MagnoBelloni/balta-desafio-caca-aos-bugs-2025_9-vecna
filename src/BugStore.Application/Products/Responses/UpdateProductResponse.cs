using BugStore.Domain.Entities;

namespace BugStore.Application.Products.Responses;

public class UpdateProductResponse
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Slug { get; set; }
    public decimal Price { get; set; }

    public static UpdateProductResponse FromProduct(Product product)
    {
        return new UpdateProductResponse
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Slug = product.Slug
        };
    }
}
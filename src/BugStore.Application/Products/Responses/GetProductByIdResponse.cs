using BugStore.Domain.Entities;

namespace BugStore.Application.Products.Responses;

public class GetProductByIdResponse
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Slug { get; set; }
    public decimal Price { get; set; }

    public static GetProductByIdResponse FromProduct(Product product)
    {
        return new GetProductByIdResponse()
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Slug = product.Slug,
            Price = product.Price
        };
    }
}
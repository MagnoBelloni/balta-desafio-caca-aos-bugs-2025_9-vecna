using BugStore.Domain.Entities;

namespace BugStore.Application.Products.Responses;

public class GetProductResponse
{
    public IEnumerable<GetProductByIdResponse> Products { get; set; } = [];

    public static GetProductResponse FromProduct(IEnumerable<Product> product)
    {
        return new GetProductResponse
        {
            Products = product.Select(GetProductByIdResponse.FromProduct)
        };
    }
}
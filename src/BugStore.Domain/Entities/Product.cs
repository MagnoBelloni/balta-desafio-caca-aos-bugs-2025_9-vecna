namespace BugStore.Domain.Entities;

public class Product : BaseEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Slug { get; set; }
    public decimal Price { get; set; }
}
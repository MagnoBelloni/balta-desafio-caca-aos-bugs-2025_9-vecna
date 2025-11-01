namespace BugStore.Domain.Entities;

public class OrderLine : BaseEntity
{
    public OrderLine()
    {

    }

    public OrderLine(int quantity, Guid productId, Product product)
    {
        Quantity = quantity;
        ProductId = productId;
        Total = Quantity * product.Price;
    }

    public Guid OrderId { get; set; }

    public int Quantity { get; private set; }
    public decimal Total { get; private set; }

    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }
}
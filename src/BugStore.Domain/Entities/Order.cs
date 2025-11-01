namespace BugStore.Domain.Entities;

public class Order : BaseEntity
{
    public Order()
    {

    }

    public Order(Guid customerId, List<OrderLine> lines)
    {
        CustomerId = customerId;
        Lines = lines;
        TotalAmount = Lines.Sum(x => x.Total);
    }

    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }

    public List<OrderLine> Lines { get; private set; } = [];
    public decimal TotalAmount { get; private set; }
}
namespace BugStore.Domain.Entities;

public class Customer : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime BirthDate { get; set; }

    public ICollection<Order> Orders { get; set; } = [];
}
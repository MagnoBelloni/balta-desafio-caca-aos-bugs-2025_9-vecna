namespace BugStore.Domain.Responses.Reports
{
    public class BestCustomerResponse
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal SpentAmount { get; set; }
    }
}

namespace OrderFlow.Document.Models.Response;

public class GetOrderItemsResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}
namespace OrderFlow.Document.Models.Request;

public class CreateOrderItemRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
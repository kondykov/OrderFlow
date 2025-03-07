namespace OrderFlow.Document.Models.Request;

public class UpdateOrderRequest
{
    public int OrderId { get; set; }
    public string Status { get; set; }
}
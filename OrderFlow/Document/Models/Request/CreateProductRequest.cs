namespace OrderFlow.Document.Models.Request;

public class CreateProductRequest
{
    public string Title { get; set; }
    public string Article { get; set; }
    public decimal Price { get; set; }
}
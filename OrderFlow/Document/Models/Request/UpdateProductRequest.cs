namespace OrderFlow.Document.Models.Request;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Article { get; set; }
    public decimal Price { get; set; }
}
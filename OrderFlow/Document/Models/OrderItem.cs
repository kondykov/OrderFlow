using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderFlow.Document.Models;

public class OrderItem
{
    [Key] public int Id { get; set; }
    public int OrderId { get; set; }
    [JsonIgnore] public Order Order { get; set; }

    public int ProductId { get; set; }
    [JsonIgnore] public Product Product { get; set; }
    public int Quantity { get; set; }
}
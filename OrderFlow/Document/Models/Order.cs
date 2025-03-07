using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Document.Models;

public class Order
{
    [Key] public int Id { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public OrderStatus? Status { get; set; } = OrderStatus.Opened;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
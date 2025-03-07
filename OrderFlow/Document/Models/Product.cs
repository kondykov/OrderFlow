using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OrderFlow.Document.Models;

public class Product
{
    [Key] public int Id { get; set; }
    public string Title { get; set; }
    public string Article { get; set; }
    public decimal Price { get; set; }
    public string FormattedPrice => Price.ToString("C2");
}
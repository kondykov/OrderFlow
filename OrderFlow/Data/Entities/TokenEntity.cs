using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderFlow.Models.Identity;

namespace OrderFlow.Data.Entities;

public class TokenEntity : Timestamps
{
    [Key] public int Id { get; set; }

    [ForeignKey("ApplicationUser")] public string UserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
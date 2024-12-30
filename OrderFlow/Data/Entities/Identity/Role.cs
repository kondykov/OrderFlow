using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Data.Entities.Identity;

public class Role : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}
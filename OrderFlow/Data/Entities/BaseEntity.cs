using System.ComponentModel.DataAnnotations;

namespace OrderFlow.Data.Entities;

public class BaseEntity
{
    [Key] public int Id { get; set; }
}
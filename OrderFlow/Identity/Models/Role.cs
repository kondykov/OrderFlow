using Microsoft.AspNetCore.Identity;

namespace OrderFlow.Identity.Models;

public class Role : IdentityRole
{
    public virtual string? ParentRole { get; set; }
}
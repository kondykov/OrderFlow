namespace OrderFlow.Models.Identity.Roles;

public class Admin : ApplicationRole
{
    public override string? Name { get; set; } = "Admin";
    public override string? NormalizedName => "Administrator";
}
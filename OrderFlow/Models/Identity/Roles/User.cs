namespace OrderFlow.Models.Identity.Roles;

public class User : ApplicationRole
{
    public override string? Name { get; set; } = "User";
    public override string? NormalizedName => "USER";
}
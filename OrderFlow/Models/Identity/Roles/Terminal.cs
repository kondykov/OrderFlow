namespace OrderFlow.Models.Identity.Roles;

public class Terminal : ApplicationRole
{
    public override string Name { get; set; } = "Terminal";
    public override string NormalizedName => "TERMINAL";
}
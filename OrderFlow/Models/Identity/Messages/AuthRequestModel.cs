namespace OrderFlow.Models.Identity.Messages;

public class AuthRequestModel
{
    public string? Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string RoleCode { get; set; }
}
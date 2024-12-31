namespace OrderFlow.Models.Identity.Messages;

public class RegRequestModel
{
    public string? Email { get; set; }
    public required string Password { get; set; }
    public required string Username { get; set; }
}
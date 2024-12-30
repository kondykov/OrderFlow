namespace OrderFlow.Models.Identity.Messages.Requests;

public class RequestAuthMessage
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string RoleCode { get; set; }
}
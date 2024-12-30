namespace OrderFlow.Models.Identity.Messages.Requests;

public class RequestRegMessage
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
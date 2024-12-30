namespace OrderFlow.Models.Identity.Messages.Requests;

public class RequestAddTypeMessage
{
    public required string Code { get; set; }
    public required string Name { get; set; }
}
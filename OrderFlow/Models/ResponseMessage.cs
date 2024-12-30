namespace OrderFlow.Models;

public class ResponseMessage(string? message = null)
{
    public virtual string? Message { get; set; } = message;
}
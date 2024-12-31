namespace OrderFlow.Models;

public class OperationResult<T>
{
    public virtual T? Data { get; init; }
    public virtual string? DataMessage { get; init; }
    public virtual string? ErrorMessage { get; init; }
    public virtual bool IsSuccessful => ErrorMessage == null;
}
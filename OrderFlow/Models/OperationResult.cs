namespace OrderFlow.Models;

public class OperationResult<T>
{
    public T? Data { get; init; }
    public int? StatusCode { get; init; }
    public string? ErrorMessage { get; init; }
    public bool IsSuccessful => ErrorMessage == null;
}
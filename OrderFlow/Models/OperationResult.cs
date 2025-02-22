namespace OrderFlow.Models;

public class OperationResult<T>
{
    public T? Data { get; init; }
    public string? Error { get; init; }
    public bool IsSuccessful => Error == null;
}

public class OperationResult<T, TE>
{
    public T? Data { get; init; }
    public TE? Error { get; init; }
    public bool IsSuccessful => Error == null;
}
using Newtonsoft.Json;

namespace OrderFlow.Models;

public class OperationResult<T>
{
    public T? Data { get; init; }
    public required int StatusCode { get; init; }
    public string? Error { get; init; }
    [JsonProperty("is_successful")] public bool IsSuccessful => Error == null;
}
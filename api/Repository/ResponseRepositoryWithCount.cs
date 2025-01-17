namespace api.Repository;

public class ResponseRepositoryWithCount<T>
{
    public int Count { get; set; }
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
}
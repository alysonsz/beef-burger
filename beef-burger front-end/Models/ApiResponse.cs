namespace BeefBurger.FrontEnd.Models;

public class ApiResponse<T>
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public int? Count { get; set; }
}

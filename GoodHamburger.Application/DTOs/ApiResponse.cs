namespace GoodHamburger.Application.DTOs;

public class ApiResponse<T>
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public T? Data { get; set; }
    public int? Count { get; set; }
    public string? Error { get; set; }

    public static ApiResponse<T> Ok(string message, T data, int? count = null)
    {
        return new ApiResponse<T>
        {
            Message = message,
            Success = true,
            Data = data,
            Count = count
        };
    }

    public static ApiResponse<T> NotFound(string message, string error = "Recurso não encontrado")
    {
        return new ApiResponse<T>
        {
            Message = message,
            Success = false,
            Error = error
        };
    }

    public static ApiResponse<T> BadRequest(string message, string error)
    {
        return new ApiResponse<T>
        {
            Message = message,
            Success = false,
            Error = error
        };
    }

    public static ApiResponse<T> Created(string message, T data, int? count = null)
    {
        return new ApiResponse<T>
        {
            Message = message,
            Success = true,
            Data = data,
            Count = count
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public new static ApiResponse Ok(string message, object? data = null, int? count = null)
    {
        return new ApiResponse
        {
            Message = message,
            Success = true,
            Data = data,
            Count = count
        };
    }

    public new static ApiResponse NotFound(string message, string error = "Recurso não encontrado")
    {
        return new ApiResponse
        {
            Message = message,
            Success = false,
            Error = error
        };
    }

    public new static ApiResponse BadRequest(string message, string error)
    {
        return new ApiResponse
        {
            Message = message,
            Success = false,
            Error = error
        };
    }

    public static ApiResponse Created(string message, object? data = null)
    {
        return new ApiResponse
        {
            Message = message,
            Success = true,
            Data = data
        };
    }
}

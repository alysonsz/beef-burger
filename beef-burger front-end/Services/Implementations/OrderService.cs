using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BeefBurger.FrontEnd.Models;
using BeefBurger.FrontEnd.Services.Contracts;

namespace BeefBurger.FrontEnd.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public OrderService(HttpClient httpClient, JsonSerializerOptions jsonOptions)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions;
    }

    public async Task<ApiResponse<OrderDto>> CreateAsync(CreateOrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/orders", request, _jsonOptions);
        return await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>(_jsonOptions) ?? new ApiResponse<OrderDto>();
    }

    public async Task<ApiResponse<List<OrderDto>>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<OrderDto>>>("api/v1/orders", _jsonOptions)
               ?? new ApiResponse<List<OrderDto>>();
    }

    public async Task<ApiResponse<OrderDto>> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<OrderDto>>($"api/v1/orders/{id}", _jsonOptions)
               ?? new ApiResponse<OrderDto>();
    }

    public async Task<ApiResponse<OrderDto>> UpdateAsync(int id, UpdateOrderRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/v1/orders/{id}", request, _jsonOptions);
        return await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>(_jsonOptions) ?? new ApiResponse<OrderDto>();
    }

    public async Task<ApiResponse<OrderDto>> UpdateStatusAsync(int id, int statusId)
    {
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/v1/orders/{id}/status/{statusId}");
        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>(_jsonOptions) ?? new ApiResponse<OrderDto>();
    }

    public async Task<ApiResponse<object>> RemoveItemsAsync(int id, RemoveOrderItemsRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/orders/{id}/items")
        {
            Content = JsonContent.Create(request, options: _jsonOptions)
        };
        var response = await _httpClient.SendAsync(httpRequest);
        return await response.Content.ReadFromJsonAsync<ApiResponse<object>>(_jsonOptions) ?? new ApiResponse<object>();
    }

    public async Task<ApiResponse<object>> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/orders/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<object>>(_jsonOptions) ?? new ApiResponse<object>();
    }
}

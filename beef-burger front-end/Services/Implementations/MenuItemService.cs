using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BeefBurger.FrontEnd.Models;
using BeefBurger.FrontEnd.Services.Contracts;

namespace BeefBurger.FrontEnd.Services.Implementations;

public class MenuItemService : IMenuItemService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public MenuItemService(HttpClient httpClient, JsonSerializerOptions jsonOptions)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions;
    }

    public async Task<ApiResponse<List<MenuItemEntityDto>>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemEntityDto>>>("api/v1/menuitems", _jsonOptions) 
               ?? new ApiResponse<List<MenuItemEntityDto>>();
    }

    public async Task<ApiResponse<MenuItemEntityDto>> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<MenuItemEntityDto>>($"api/v1/menuitems/{id}", _jsonOptions)
               ?? new ApiResponse<MenuItemEntityDto>();
    }

    public async Task<ApiResponse<List<MenuItemEntityDto>>> GetByCategoryAsync(int categoryId)
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemEntityDto>>>($"api/v1/menuitems/category/{categoryId}", _jsonOptions)
               ?? new ApiResponse<List<MenuItemEntityDto>>();
    }

    public async Task<ApiResponse<List<MenuItemEntityDto>>> SearchAsync(string name)
    {
        return await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemEntityDto>>>($"api/v1/menuitems/search?name={name}", _jsonOptions)
               ?? new ApiResponse<List<MenuItemEntityDto>>();
    }

    public async Task<ApiResponse<MenuItemEntityDto>> CreateAsync(CreateMenuItemRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/menuitems", request, _jsonOptions);
        return await response.Content.ReadFromJsonAsync<ApiResponse<MenuItemEntityDto>>(_jsonOptions) 
               ?? new ApiResponse<MenuItemEntityDto>();
    }

    public async Task<ApiResponse<MenuItemEntityDto>> UpdateAsync(int id, UpdateMenuItemRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/v1/menuitems/{id}", request, _jsonOptions);
        return await response.Content.ReadFromJsonAsync<ApiResponse<MenuItemEntityDto>>(_jsonOptions) 
               ?? new ApiResponse<MenuItemEntityDto>();
    }

    public async Task<ApiResponse<object>> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/menuitems/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<object>>(_jsonOptions) 
               ?? new ApiResponse<object>();
    }
}

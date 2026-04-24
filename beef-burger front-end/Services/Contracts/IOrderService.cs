using System.Collections.Generic;
using System.Threading.Tasks;
using BeefBurger.FrontEnd.Models;

namespace BeefBurger.FrontEnd.Services.Contracts;

public interface IOrderService
{
    Task<ApiResponse<OrderDto>> CreateAsync(CreateOrderRequest request);
    Task<ApiResponse<List<OrderDto>>> GetAllAsync();
    Task<ApiResponse<OrderDto>> GetByIdAsync(int id);
    Task<ApiResponse<OrderDto>> UpdateAsync(int id, UpdateOrderRequest request);
    Task<ApiResponse<OrderDto>> UpdateStatusAsync(int id, int statusId);
    Task<ApiResponse<object>> RemoveItemsAsync(int id, RemoveOrderItemsRequest request);
    Task<ApiResponse<object>> DeleteAsync(int id);
}

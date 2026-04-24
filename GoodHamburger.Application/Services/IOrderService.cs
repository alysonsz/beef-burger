using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain;

namespace GoodHamburger.Application.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderRequest request);
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderRequest request);
    Task<object?> RemoveOrderItemsAsync(int id, RemoveOrderItemsRequest request);
    Task<OrderDto?> UpdateOrderStatusAsync(int id, OrderStatus status);
    Task<bool> DeleteOrderAsync(int id);
}

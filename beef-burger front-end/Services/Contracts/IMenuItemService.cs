using System.Collections.Generic;
using System.Threading.Tasks;
using BeefBurger.FrontEnd.Models;

namespace BeefBurger.FrontEnd.Services.Contracts;

public interface IMenuItemService
{
    Task<ApiResponse<List<MenuItemEntityDto>>> GetAllAsync();
    Task<ApiResponse<MenuItemEntityDto>> GetByIdAsync(int id);
    Task<ApiResponse<List<MenuItemEntityDto>>> GetByCategoryAsync(int categoryId);
    Task<ApiResponse<List<MenuItemEntityDto>>> SearchAsync(string name);
    Task<ApiResponse<MenuItemEntityDto>> CreateAsync(CreateMenuItemRequest request);
    Task<ApiResponse<MenuItemEntityDto>> UpdateAsync(int id, UpdateMenuItemRequest request);
    Task<ApiResponse<object>> DeleteAsync(int id);
}

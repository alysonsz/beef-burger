using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain;

namespace GoodHamburger.Application.Services;

public interface IMenuItemService
{
    Task<List<MenuItemEntityDto>> GetAllMenuItemsAsync();
    Task<MenuItemEntityDto?> GetMenuItemByIdAsync(int id);
    Task<List<MenuItemEntityDto>> GetMenuItemsByCategoryAsync(MenuItemCategory category);
    Task<List<MenuItemEntityDto>> SearchMenuItemsByNameAsync(string name);
    Task<MenuItemEntityDto> CreateMenuItemAsync(CreateMenuItemRequest request);
    Task<MenuItemEntityDto?> UpdateMenuItemAsync(int id, UpdateMenuItemRequest request);
    Task<bool> DeleteMenuItemAsync(int id);
}

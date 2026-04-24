using GoodHamburger.Domain;

namespace GoodHamburger.Infrastructure;

public interface IMenuItemRepository
{
    Task<List<MenuItemEntity>> GetAllAsync();
    Task<MenuItemEntity?> GetByIdAsync(int id);
    Task<List<MenuItemEntity>> GetByCategoryAsync(MenuItemCategory category);
    Task<MenuItemEntity> AddAsync(MenuItemEntity menuItem);
    Task<MenuItemEntity?> UpdateAsync(MenuItemEntity menuItem);
    Task DeleteAsync(int id);
}

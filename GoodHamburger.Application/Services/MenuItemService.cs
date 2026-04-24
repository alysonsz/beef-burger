using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain;
using GoodHamburger.Infrastructure;

namespace GoodHamburger.Application.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private static readonly int[] FixedItemIds = { (int)MenuItem.XBurger, (int)MenuItem.XEgg, (int)MenuItem.XBacon, (int)MenuItem.Fries, (int)MenuItem.SoftDrink };

    public MenuItemService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<List<MenuItemEntityDto>> GetAllMenuItemsAsync()
    {
        var customItems = (await _menuItemRepository.GetAllAsync())
            .Where(m => !IsFixedItemId(m.Id))
            .Select(MapToDto)
            .ToList();

        return GetFixedMenuItems().Concat(customItems).OrderBy(m => m.Category).ThenBy(m => m.Name).ToList();
    }

    public async Task<MenuItemEntityDto?> GetMenuItemByIdAsync(int id)
    {
        if (IsFixedItemId(id))
            return GetFixedMenuItem(id);

        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        return menuItem is null ? null : MapToDto(menuItem);
    }

    public async Task<List<MenuItemEntityDto>> GetMenuItemsByCategoryAsync(MenuItemCategory category)
    {
        var customItems = (await _menuItemRepository.GetByCategoryAsync(category))
            .Where(m => !IsFixedItemId(m.Id))
            .Select(MapToDto)
            .ToList();

        return GetFixedMenuItems().Where(m => m.Category == category).Concat(customItems).OrderBy(m => m.Name).ToList();
    }

    public async Task<List<MenuItemEntityDto>> SearchMenuItemsByNameAsync(string name)
    {
        var fixedItems = GetFixedMenuItems()
            .Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var customItems = (await _menuItemRepository.GetAllAsync())
            .Where(m => !IsFixedItemId(m.Id) && m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .Select(MapToDto)
            .ToList();

        return fixedItems.Concat(customItems).OrderBy(m => m.Name).ToList();
    }

    public async Task<MenuItemEntityDto> CreateMenuItemAsync(CreateMenuItemRequest request)
    {
        var menuItem = new MenuItemEntity
        {
            Name = request.Name,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            Category = request.Category
        };

        var created = await _menuItemRepository.AddAsync(menuItem);

        if (IsFixedItemId(created.Id))
        {
            await _menuItemRepository.DeleteAsync(created.Id);
            throw new InvalidOperationException("Não é possível criar item com ID fixo reservado para itens do sistema.");
        }

        return MapToDto(created);
    }

    public async Task<MenuItemEntityDto?> UpdateMenuItemAsync(int id, UpdateMenuItemRequest request)
    {
        if (IsFixedItemId(id))
            throw new InvalidOperationException("Não é possível atualizar itens fixos do sistema.");

        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem is null)
            return null;

        menuItem.Name = request.Name;
        menuItem.Price = request.Price;
        menuItem.ImageUrl = request.ImageUrl;
        menuItem.Category = request.Category;

        var updated = await _menuItemRepository.UpdateAsync(menuItem);
        return updated is null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        if (IsFixedItemId(id))
            throw new InvalidOperationException("Não é possível excluir itens fixos do sistema.");

        var menuItem = await _menuItemRepository.GetByIdAsync(id);
        if (menuItem is null)
            return false;

        await _menuItemRepository.DeleteAsync(id);
        return true;
    }

    private static bool IsFixedItemId(int id) => FixedItemIds.Contains(id);

    private static MenuItemEntityDto? GetFixedMenuItem(int id)
    {
        if (!Enum.IsDefined(typeof(MenuItem), id))
            return null;

        var menuItem = (MenuItem)id;
        return new MenuItemEntityDto
        {
            Id = id,
            Name = menuItem.GetName(),
            Price = menuItem.GetPrice(),
            ImageUrl = menuItem.GetImageUrl(),
            Category = GetMenuItemCategory(menuItem),
            CreatedAt = DateTime.Now
        };
    }

    private static List<MenuItemEntityDto> GetFixedMenuItems()
    {
        return Enum.GetValues<MenuItem>().Select(item => new MenuItemEntityDto
        {
            Id = (int)item,
            Name = item.GetName(),
            Price = item.GetPrice(),
            ImageUrl = item.GetImageUrl(),
            Category = GetMenuItemCategory(item),
            CreatedAt = DateTime.Now
        }).ToList();
    }

    private static MenuItemCategory GetMenuItemCategory(MenuItem item) => item.GetCategory() switch
    {
        "Sandwich" => MenuItemCategory.Sandwich,
        "Fries" => MenuItemCategory.Fries,
        "Drink" => MenuItemCategory.Drink,
        _ => throw new ArgumentException($"Unknown category for {item}")
    };

    private static MenuItemEntityDto MapToDto(MenuItemEntity menuItem)
    {
        return new MenuItemEntityDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Price = menuItem.Price,
            ImageUrl = menuItem.ImageUrl,
            Category = menuItem.Category,
            CreatedAt = menuItem.CreatedAt
        };
    }
}

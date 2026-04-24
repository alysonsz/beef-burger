using GoodHamburger.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly AppDbContext _context;

    public MenuItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuItemEntity>> GetAllAsync()
    {
        return await _context.MenuItems.OrderBy(m => m.Category).ThenBy(m => m.Name).ToListAsync();
    }

    public async Task<MenuItemEntity?> GetByIdAsync(int id)
    {
        return await _context.MenuItems.FindAsync(id);
    }

    public async Task<List<MenuItemEntity>> GetByCategoryAsync(MenuItemCategory category)
    {
        return await _context.MenuItems.Where(m => m.Category == category).OrderBy(m => m.Name).ToListAsync();
    }

    public async Task<MenuItemEntity> AddAsync(MenuItemEntity menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
        return menuItem;
    }

    public async Task<MenuItemEntity?> UpdateAsync(MenuItemEntity menuItem)
    {
        var existing = await _context.MenuItems.FindAsync(menuItem.Id);
        if (existing is null)
            return null;

        existing.Name = menuItem.Name;
        existing.Price = menuItem.Price;
        existing.ImageUrl = menuItem.ImageUrl;
        existing.Category = menuItem.Category;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task DeleteAsync(int id)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem is not null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}

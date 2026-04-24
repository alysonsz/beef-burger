using System;

namespace BeefBurger.FrontEnd.Models;

public class MenuItemDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class MenuItemEntityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public MenuItemCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
}

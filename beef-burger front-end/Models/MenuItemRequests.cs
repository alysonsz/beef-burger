namespace BeefBurger.FrontEnd.Models;

public class CreateMenuItemRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public MenuItemCategory Category { get; set; }
}

public class UpdateMenuItemRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public MenuItemCategory Category { get; set; }
}

public class RemoveOrderItemsRequest
{
    public List<int> ItemIds { get; set; } = new();
}

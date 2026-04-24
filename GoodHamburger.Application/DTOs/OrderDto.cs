using GoodHamburger.Domain;

namespace GoodHamburger.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public MenuItemDto? Sandwich { get; set; }
    public MenuItemDto? Fries { get; set; }
    public MenuItemDto? Drink { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public OrderStatus Status { get; set; }
}

public class MenuItemDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class CreateOrderRequest
{
    public int? Sandwich { get; set; }
    public int? Fries { get; set; }
    public int? Drink { get; set; }
}

public class UpdateOrderRequest
{
    public int? Sandwich { get; set; }
    public int? Fries { get; set; }
    public int? Drink { get; set; }
}

public class RemoveOrderItemsRequest
{
    public List<int> ItemIds { get; set; } = new();
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

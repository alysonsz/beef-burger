using System;

namespace BeefBurger.FrontEnd.Models;

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

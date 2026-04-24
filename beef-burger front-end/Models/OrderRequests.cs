namespace BeefBurger.FrontEnd.Models;

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

namespace GoodHamburger.Domain;

public enum OrderStatus
{
    Pending = 1,
    Preparing = 2,
    Ready = 3,
    Delivered = 4,
    Cancelled = 5
}

public class OrderItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class Order
{
    public int Id { get; private set; }
    public OrderItem? Sandwich { get; private set; }
    public OrderItem? Fries { get; private set; }
    public OrderItem? Drink { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    private Order() { }

    public Order(OrderItem? sandwich, OrderItem? fries, OrderItem? drink)
    {
        Sandwich = sandwich;
        Fries = fries;
        Drink = drink;
        CreatedAt = DateTime.Now;
        Status = OrderStatus.Pending;
        ValidateItems();
        CalculateTotals();
    }

    public void AddItems(OrderItem? sandwich, OrderItem? fries, OrderItem? drink)
    {
        if (sandwich is not null)
            Sandwich = sandwich;
        if (fries is not null)
            Fries = fries;
        if (drink is not null)
            Drink = drink;
        ValidateItems();
        CalculateTotals();
    }

    public void RemoveItem(int itemId)
    {
        if (Sandwich is not null && Sandwich.Id == itemId)
            Sandwich = null;
        if (Fries is not null && Fries.Id == itemId)
            Fries = null;
        if (Drink is not null && Drink.Id == itemId)
            Drink = null;
        CalculateTotals();
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }

    private void ValidateItems()
    {
        if (Sandwich is not null && Sandwich.Category != "Sandwich")
            throw new InvalidOperationException($"O item '{Sandwich.Name}' não é um sanduíche.");
        if (Fries is not null && Fries.Category != "Fries")
            throw new InvalidOperationException($"O item '{Fries.Name}' não é uma batata frita.");
        if (Drink is not null && Drink.Category != "Drink")
            throw new InvalidOperationException($"O item '{Drink.Name}' não é uma bebida.");

        var ids = new List<int>();
        if (Sandwich is not null) ids.Add(Sandwich.Id);
        if (Fries is not null) ids.Add(Fries.Id);
        if (Drink is not null) ids.Add(Drink.Id);

        if (ids.Distinct().Count() != ids.Count)
            throw new InvalidOperationException("Um pedido não pode conter itens duplicados.");
    }

    private void CalculateTotals()
    {
        var items = new List<OrderItem?>();
        if (Sandwich is not null) items.Add(Sandwich);
        if (Fries is not null) items.Add(Fries);
        if (Drink is not null) items.Add(Drink);

        Subtotal = items.Sum(item => item?.Price ?? 0);
        Discount = CalculateDiscount(items);
        Total = Subtotal - Discount;
    }

    private decimal CalculateDiscount(List<OrderItem?> items)
    {
        var hasSandwich = items.Any(i => i is not null && i.Category == "Sandwich");
        var hasFries = items.Any(i => i is not null && i.Category == "Fries");
        var hasDrink = items.Any(i => i is not null && i.Category == "Drink");

        if (hasSandwich && hasFries && hasDrink)
            return Subtotal * 0.20m;
        if (hasSandwich && hasDrink)
            return Subtotal * 0.15m;
        if (hasSandwich && hasFries)
            return Subtotal * 0.10m;

        return 0;
    }
}

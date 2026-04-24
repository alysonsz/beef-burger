using GoodHamburger.Domain;
using Xunit;

namespace GoodHamburger.Tests;

public class OrderTests
{
    private static OrderItem CreateOrderItem(MenuItem menuItem)
    {
        return new OrderItem
        {
            Id = (int)menuItem,
            Name = menuItem.GetName(),
            Price = menuItem.GetPrice(),
            Category = menuItem.GetCategory()
        };
    }

    [Fact]
    public void CreateOrder_WithValidItems_CalculatesCorrectTotals()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var fries = CreateOrderItem(MenuItem.Fries);
        var drink = CreateOrderItem(MenuItem.SoftDrink);

        var order = new Order(sandwich, fries, drink);

        Assert.Equal(5.00m + 2.00m + 2.50m, order.Subtotal);
        Assert.Equal((5.00m + 2.00m + 2.50m) * 0.20m, order.Discount);
        Assert.Equal((5.00m + 2.00m + 2.50m) * 0.80m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void CreateOrder_WithSandwichAndDrink_Applies15PercentDiscount()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var drink = CreateOrderItem(MenuItem.SoftDrink);

        var order = new Order(sandwich, null, drink);

        Assert.Equal(5.00m + 2.50m, order.Subtotal);
        Assert.Equal((5.00m + 2.50m) * 0.15m, order.Discount);
        Assert.Equal((5.00m + 2.50m) * 0.85m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void CreateOrder_WithSandwichAndFries_Applies10PercentDiscount()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var fries = CreateOrderItem(MenuItem.Fries);

        var order = new Order(sandwich, fries, null);

        Assert.Equal(5.00m + 2.00m, order.Subtotal);
        Assert.Equal((5.00m + 2.00m) * 0.10m, order.Discount);
        Assert.Equal((5.00m + 2.00m) * 0.90m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void CreateOrder_WithOnlySandwich_NoDiscount()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);

        var order = new Order(sandwich, null, null);

        Assert.Equal(5.00m, order.Subtotal);
        Assert.Equal(0m, order.Discount);
        Assert.Equal(5.00m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void CreateOrder_WithOnlyFries_NoDiscount()
    {
        var fries = CreateOrderItem(MenuItem.Fries);

        var order = new Order(null, fries, null);

        Assert.Equal(2.00m, order.Subtotal);
        Assert.Equal(0m, order.Discount);
        Assert.Equal(2.00m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void CreateOrder_WithOnlyDrink_NoDiscount()
    {
        var drink = CreateOrderItem(MenuItem.SoftDrink);

        var order = new Order(null, null, drink);

        Assert.Equal(2.50m, order.Subtotal);
        Assert.Equal(0m, order.Discount);
        Assert.Equal(2.50m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void CreateOrder_WithEmptyOrder_NoDiscount()
    {
        var order = new Order(null, null, null);

        Assert.Equal(0m, order.Subtotal);
        Assert.Equal(0m, order.Discount);
        Assert.Equal(0m, order.Total);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void UpdateStatus_UpdatesOrderStatus()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var order = new Order(sandwich, null, null);

        order.UpdateStatus(OrderStatus.Preparing);

        Assert.Equal(OrderStatus.Preparing, order.Status);
    }

    [Fact]
    public void CreateOrder_WithInvalidSandwich_ThrowsException()
    {
        var sandwich = new OrderItem { Id = 1, Name = "Invalid", Price = 10m, Category = "Fries" };

        Assert.Throws<InvalidOperationException>(() => new Order(sandwich, null, null));
    }

    [Fact]
    public void CreateOrder_WithInvalidFries_ThrowsException()
    {
        var fries = new OrderItem { Id = 1, Name = "Invalid", Price = 10m, Category = "Sandwich" };

        Assert.Throws<InvalidOperationException>(() => new Order(null, fries, null));
    }

    [Fact]
    public void CreateOrder_WithInvalidDrink_ThrowsException()
    {
        var drink = new OrderItem { Id = 1, Name = "Invalid", Price = 10m, Category = "Sandwich" };

        Assert.Throws<InvalidOperationException>(() => new Order(null, null, drink));
    }

    [Fact]
    public void AddItems_ShouldRecalculateDiscount()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var order = new Order(sandwich, null, null);
        var fries = CreateOrderItem(MenuItem.Fries);
        var drink = CreateOrderItem(MenuItem.SoftDrink);

        order.AddItems(null, fries, drink);

        Assert.Equal(5.00m + 2.00m + 2.50m, order.Subtotal);
        Assert.Equal((5.00m + 2.00m + 2.50m) * 0.20m, order.Discount);
        Assert.Equal((5.00m + 2.00m + 2.50m) * 0.80m, order.Total);
    }

    [Fact]
    public void CreateOrder_WithDuplicateItems_ThrowsException()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var fries = new OrderItem { Id = 500, Name = "XBurger", Price = 5.00m, Category = "Fries" };

        Assert.Throws<InvalidOperationException>(() => new Order(sandwich, fries, null));
    }

    [Fact]
    public void AddItems_ReplacesItemsOfSameCategory()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var order = new Order(sandwich, null, null);
        var additionalSandwich = CreateOrderItem(MenuItem.XEgg);

        order.AddItems(additionalSandwich, null, null);

        Assert.Equal(4.50m, order.Subtotal);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveSpecificItem()
    {
        var sandwich = CreateOrderItem(MenuItem.XBurger);
        var fries = CreateOrderItem(MenuItem.Fries);
        var order = new Order(sandwich, fries, null);

        order.RemoveItem(500);

        Assert.Null(order.Sandwich);
        Assert.NotNull(order.Fries);
        Assert.Equal(2.00m, order.Subtotal);
    }

    [Theory]
    [InlineData(MenuItem.XBurger)]
    [InlineData(MenuItem.XEgg)]
    [InlineData(MenuItem.XBacon)]
    public void MenuItem_GetPrice_ShouldReturnCorrectPrice(MenuItem item)
    {
        var price = item.GetPrice();

        Assert.True(price > 0);
    }

    [Theory]
    [InlineData(MenuItem.XBurger, "Sandwich")]
    [InlineData(MenuItem.XEgg, "Sandwich")]
    [InlineData(MenuItem.XBacon, "Sandwich")]
    [InlineData(MenuItem.Fries, "Fries")]
    [InlineData(MenuItem.SoftDrink, "Drink")]
    public void MenuItem_GetCategory_ShouldReturnCorrectCategory(MenuItem item, string expectedCategory)
    {
        var category = item.GetCategory();

        Assert.Equal(expectedCategory, category);
    }
}

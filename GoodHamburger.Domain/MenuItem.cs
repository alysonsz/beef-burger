namespace GoodHamburger.Domain;

public enum MenuItemCategory
{
    Sandwich = 1,
    Fries = 2,
    Drink = 3
}

public class MenuItemEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public MenuItemCategory Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public enum MenuItem
{
    XBurger = 500,
    XEgg = 450,
    XBacon = 700,
    Fries = 200,
    SoftDrink = 250
}

public static class MenuItemExtensions
{
    public static decimal GetPrice(this MenuItem item)
    {
        return item switch
        {
            MenuItem.XBurger => 5.00m,
            MenuItem.XEgg => 4.50m,
            MenuItem.XBacon => 7.00m,
            MenuItem.Fries => 2.00m,
            MenuItem.SoftDrink => 2.50m,
            _ => throw new ArgumentException("Invalid menu item")
        };
    }

    public static string GetCategory(this MenuItem item)
    {
        return item switch
        {
            MenuItem.XBurger or MenuItem.XEgg or MenuItem.XBacon => "Sandwich",
            MenuItem.Fries => "Fries",
            MenuItem.SoftDrink => "Drink",
            _ => throw new ArgumentException("Invalid menu item")
        };
    }

    public static string GetName(this MenuItem item)
    {
        return item switch
        {
            MenuItem.XBurger => "X Burger",
            MenuItem.XEgg => "X Egg",
            MenuItem.XBacon => "X Bacon",
            MenuItem.Fries => "Fries",
            MenuItem.SoftDrink => "Soft drink",
            _ => throw new ArgumentException("Invalid menu item")
        };
    }

    public static string GetImageUrl(this MenuItem item)
    {
        return item switch
        {
            MenuItem.XBurger => "https://img.freepik.com/fotos-gratis/hamburguer-de-queijo-classico-com-costeleta-de-carne-legumes-e-cebola-isolados-em-um-fundo-branco_123827-29709.jpg?semt=ais_hybrid&w=740&q=80",
            MenuItem.XEgg => "https://img.freepik.com/fotos-gratis/um-delicioso-hamburguer-de-pequeno-almoco-com-ovo-frito-e-bacon_23-2151985469.jpg?semt=ais_hybrid&w=740&q=80",
            MenuItem.XBacon => "https://img.freepik.com/fotos-gratis/um-delicioso-cheeseburger-grelhado-com-bacon_23-2151985470.jpg?semt=ais_hybrid&w=740&q=80",
            MenuItem.Fries => "https://img.freepik.com/fotos-gratis/batatas-fritas-deliciosas-no-estudio_23-2151846534.jpg",
            MenuItem.SoftDrink => "https://img.freepik.com/fotos-gratis/servindo-uma-cola-de-uma-garrafa-para-um-copo-cheio-de-gelo_463209-157.jpg?semt=ais_hybrid&w=740&q=80",
            _ => throw new ArgumentException("Invalid menu item")
        };
    }
}

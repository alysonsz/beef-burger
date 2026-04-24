using System.Globalization;

namespace BeefBurger.FrontEnd.Extensions;

public static class CurrencyExtensions
{
    public static string FormatCurrency(this decimal value)
    {
        return value.ToString("C", new CultureInfo("pt-BR"));
    }
}

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BeefBurger.FrontEnd;
using BeefBurger.FrontEnd.Services.Contracts;
using BeefBurger.FrontEnd.Services.Implementations;
using BeefBurger.FrontEnd.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };
    return client;
});

builder.Services.AddScoped(sp =>
    new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(), new BrazilianDateTimeConverter() }
    });

builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();

await builder.Build().RunAsync();

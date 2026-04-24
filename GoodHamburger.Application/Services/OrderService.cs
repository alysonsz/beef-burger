using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain;
using GoodHamburger.Infrastructure;

namespace GoodHamburger.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private static readonly int[] FixedItemIds = { (int)MenuItem.XBurger, (int)MenuItem.XEgg, (int)MenuItem.XBacon, (int)MenuItem.Fries, (int)MenuItem.SoftDrink };

    public OrderService(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request)
    {
        var sandwich = request.Sandwich.HasValue ? await GetOrderItemAsync(request.Sandwich, MenuItemCategory.Sandwich) : null;
        var fries = request.Fries.HasValue ? await GetOrderItemAsync(request.Fries, MenuItemCategory.Fries) : null;
        var drink = request.Drink.HasValue ? await GetOrderItemAsync(request.Drink, MenuItemCategory.Drink) : null;

        var order = new Order(sandwich, fries, drink);
        await _orderRepository.AddAsync(order);

        return MapToDto(order);
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDto).ToList();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order is null ? null : MapToDto(order);
    }

    public async Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return null;

        var sandwich = request.Sandwich.HasValue ? await GetOrderItemAsync(request.Sandwich, MenuItemCategory.Sandwich) : null;
        var fries = request.Fries.HasValue ? await GetOrderItemAsync(request.Fries, MenuItemCategory.Fries) : null;
        var drink = request.Drink.HasValue ? await GetOrderItemAsync(request.Drink, MenuItemCategory.Drink) : null;

        if (sandwich is not null && order.Sandwich is not null && order.Sandwich.Id == sandwich.Id)
            throw new InvalidOperationException("Este sanduíche já está no pedido. Para alterar, selecione um sanduíche diferente.");
        if (fries is not null && order.Fries is not null && order.Fries.Id == fries.Id)
            throw new InvalidOperationException("Esta batata frita já está no pedido. Para alterar, selecione uma batata frita diferente.");
        if (drink is not null && order.Drink is not null && order.Drink.Id == drink.Id)
            throw new InvalidOperationException("Esta bebida já está no pedido. Para alterar, selecione uma bebida diferente.");

        order.AddItems(sandwich, fries, drink);
        await _orderRepository.UpdateAsync(order);

        return MapToDto(order);
    }

    public async Task<object?> RemoveOrderItemsAsync(int id, RemoveOrderItemsRequest request)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return null;

        if (request.ItemIds is null || request.ItemIds.Count == 0)
            return new
            {
                deletedItems = new List<MenuItemDto>(),
                order = MapToDto(order)
            };

        var deletedItems = new List<MenuItemDto>();

        foreach (var itemId in request.ItemIds)
        {
            if (order.Sandwich is not null && order.Sandwich.Id == itemId)
            {
                deletedItems.Add(new MenuItemDto { Name = order.Sandwich.Name, Price = order.Sandwich.Price, ImageUrl = GetImageUrl(itemId) });
                order.RemoveItem(itemId);
            }

            if (order.Fries is not null && order.Fries.Id == itemId)
            {
                deletedItems.Add(new MenuItemDto { Name = order.Fries.Name, Price = order.Fries.Price, ImageUrl = GetImageUrl(itemId) });
                order.RemoveItem(itemId);
            }

            if (order.Drink is not null && order.Drink.Id == itemId)
            {
                deletedItems.Add(new MenuItemDto { Name = order.Drink.Name, Price = order.Drink.Price, ImageUrl = GetImageUrl(itemId) });
                order.RemoveItem(itemId);
            }
        }

        await _orderRepository.UpdateAsync(order);

        return new
        {
            deletedItems,
            order = MapToDto(order)
        };
    }

    public async Task<OrderDto?> UpdateOrderStatusAsync(int id, OrderStatus status)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status))
            throw new InvalidOperationException($"Status inválido: {status}. Statuses válidos: 1 (Pending), 2 (Preparing), 3 (Ready), 4 (Delivered), 5 (Cancelled)");

        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return null;

        order.UpdateStatus(status);
        await _orderRepository.UpdateAsync(order);

        return MapToDto(order);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            return false;

        await _orderRepository.DeleteAsync(id);
        return true;
    }

    private async Task<OrderItem?> GetOrderItemAsync(int? itemId, MenuItemCategory expectedCategory)
    {
        if (!itemId.HasValue)
            return null;

        var id = itemId.Value;

        // Get fixed item
        if (FixedItemIds.Contains(id) && Enum.IsDefined(typeof(MenuItem), id))
        {
            var menuItem = (MenuItem)id;
            var category = menuItem.GetCategory();
            var expectedCategoryName = expectedCategory.ToString();
            if (category != expectedCategoryName)
                throw new InvalidOperationException($"O item com ID {id} não pertence à categoria esperada.");

            return new OrderItem
            {
                Id = id,
                Name = menuItem.GetName(),
                Price = menuItem.GetPrice(),
                Category = category
            };
        }

        // Get custom item
        var customItem = await _menuItemRepository.GetByIdAsync(id);
        if (customItem is null)
            throw new InvalidOperationException($"ID de item inválido: {id}");

        if (customItem.Category != expectedCategory)
            throw new InvalidOperationException($"O item com ID {id} não pertence à categoria esperada.");

        return new OrderItem
        {
            Id = customItem.Id,
            Name = customItem.Name,
            Price = customItem.Price,
            Category = customItem.Category.ToString()
        };
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            Sandwich = order.Sandwich is not null ? new MenuItemDto
            {
                Name = order.Sandwich.Name,
                Price = order.Sandwich.Price,
                ImageUrl = GetImageUrl(order.Sandwich.Id)
            } : null,
            Fries = order.Fries is not null ? new MenuItemDto
            {
                Name = order.Fries.Name,
                Price = order.Fries.Price,
                ImageUrl = GetImageUrl(order.Fries.Id)
            } : null,
            Drink = order.Drink is not null ? new MenuItemDto
            {
                Name = order.Drink.Name,
                Price = order.Drink.Price,
                ImageUrl = GetImageUrl(order.Drink.Id)
            } : null,
            Subtotal = order.Subtotal,
            Discount = order.Discount,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Status = order.Status
        };
    }

    private static string GetImageUrl(int itemId)
    {
        if (Enum.IsDefined(typeof(MenuItem), itemId))
        {
            var menuItem = (MenuItem)itemId;
            return menuItem.GetImageUrl();
        }

        return "https://via.placeholder.com/150";
    }
}

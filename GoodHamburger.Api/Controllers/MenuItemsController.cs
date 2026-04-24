using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Services;
using GoodHamburger.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemsController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<MenuItemEntityDto>>>> GetAllMenuItems()
    {
        var menuItems = await _menuItemService.GetAllMenuItemsAsync();

        if (menuItems.Count == 0)
            return Ok(ApiResponse<List<MenuItemEntityDto>>.Ok("Nenhum item de cardápio encontrado.", menuItems, 0));

        return Ok(ApiResponse<List<MenuItemEntityDto>>.Ok("Itens de cardápio listados com sucesso!", menuItems, menuItems.Count));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<MenuItemEntityDto>>> GetMenuItemById(int id)
    {
        var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
        if (menuItem is null)
            return NotFound(ApiResponse<MenuItemEntityDto>.NotFound($"Item de cardápio com ID {id} não encontrado.", "Item não encontrado"));

        return Ok(ApiResponse<MenuItemEntityDto>.Ok("Item de cardápio encontrado com sucesso!", menuItem, 1));
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<ApiResponse<List<MenuItemEntityDto>>>> GetMenuItemsByCategory(MenuItemCategory category)
    {
        var menuItems = await _menuItemService.GetMenuItemsByCategoryAsync(category);

        if (menuItems.Count == 0)
            return Ok(ApiResponse<List<MenuItemEntityDto>>.Ok($"Nenhum item encontrado na categoria {category}.", menuItems, 0));

        return Ok(ApiResponse<List<MenuItemEntityDto>>.Ok($"Itens da categoria {category} listados com sucesso!", menuItems, menuItems.Count));
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<List<MenuItemEntityDto>>>> SearchMenuItemsByName([FromQuery] string name)
    {
        var menuItems = await _menuItemService.SearchMenuItemsByNameAsync(name);

        if (menuItems.Count == 0)
            return Ok(ApiResponse<List<MenuItemEntityDto>>.Ok($"Nenhum item encontrado com o nome '{name}'.", menuItems, 0));

        return Ok(ApiResponse<List<MenuItemEntityDto>>.Ok($"Itens encontrados com o nome '{name}' listados com sucesso!", menuItems, menuItems.Count));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<MenuItemEntityDto>>> CreateMenuItem([FromBody] CreateMenuItemRequest request)
    {
        try
        {
            var menuItem = await _menuItemService.CreateMenuItemAsync(request);
            return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItem.Id },
                ApiResponse<MenuItemEntityDto>.Created("Item de cardápio criado com sucesso!", menuItem));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<MenuItemEntityDto>.BadRequest($"Falha ao criar item: {ex.Message}", ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<MenuItemEntityDto>>> UpdateMenuItem(int id, [FromBody] UpdateMenuItemRequest request)
    {
        try
        {
            var menuItem = await _menuItemService.UpdateMenuItemAsync(id, request);
            if (menuItem is null)
                return NotFound(ApiResponse<MenuItemEntityDto>.NotFound($"Item de cardápio com ID {id} não encontrado.", "Item não encontrado"));

            return Ok(ApiResponse<MenuItemEntityDto>.Ok("Item de cardápio atualizado com sucesso!", menuItem));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<MenuItemEntityDto>.BadRequest($"Falha ao atualizar item: {ex.Message}", ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
    {
        var deleted = await _menuItemService.DeleteMenuItemAsync(id);
        if (!deleted)
            return NotFound(ApiResponse.NotFound($"Item de cardápio com ID {id} não encontrado.", "Item não encontrado"));

        return Ok(ApiResponse.Ok("Item de cardápio excluído com sucesso!"));
    }
}

using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Services;
using GoodHamburger.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id },
                ApiResponse<OrderDto>.Created("Pedido criado com sucesso!", order, 1));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<OrderDto>.BadRequest($"Falha ao criar pedido: {ex.Message}", ex.Message));
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();

        if (orders.Count == 0)
            return Ok(ApiResponse<List<OrderDto>>.Ok("Nenhum pedido encontrado.", orders, 0));

        return Ok(ApiResponse<List<OrderDto>>.Ok("Pedidos listados com sucesso!", orders, orders.Count));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order is null)
            return NotFound(ApiResponse<OrderDto>.NotFound($"Pedido com ID {id} não encontrado.", "Pedido não encontrado"));

        return Ok(ApiResponse<OrderDto>.Ok("Pedido encontrado com sucesso!", order, 1));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrder(int id, [FromBody] UpdateOrderRequest request)
    {
        try
        {
            var order = await _orderService.UpdateOrderAsync(id, request);
            if (order is null)
                return NotFound(ApiResponse<OrderDto>.NotFound($"Pedido com ID {id} não encontrado.", "Pedido não encontrado"));

            return Ok(ApiResponse<OrderDto>.Ok("Pedido atualizado com sucesso!", order, 1));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<OrderDto>.BadRequest($"Falha ao atualizar pedido: {ex.Message}", ex.Message));
        }
    }

    [HttpPatch("{id}/status/{status}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrderStatus(int id, OrderStatus status)
    {
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, status);
            if (order is null)
                return NotFound(ApiResponse<OrderDto>.NotFound($"Pedido com ID {id} não encontrado.", "Pedido não encontrado"));

            return Ok(ApiResponse<OrderDto>.Ok($"Status do pedido atualizado para '{status}' com sucesso!", order, 1));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<OrderDto>.BadRequest($"Falha ao atualizar status: {ex.Message}", ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteOrder(int id)
    {
        var deleted = await _orderService.DeleteOrderAsync(id);
        if (!deleted)
            return NotFound(ApiResponse.NotFound($"Pedido com ID {id} não encontrado.", "Pedido não encontrado"));

        return Ok("Pedido excluído com sucesso!");
    }

    [HttpDelete("{id}/items")]
    public async Task<ActionResult<ApiResponse>> RemoveOrderItems(int id, [FromBody] RemoveOrderItemsRequest request)
    {
        try
        {
            var result = await _orderService.RemoveOrderItemsAsync(id, request);
            if (result is null)
                return NotFound(ApiResponse.NotFound($"Pedido com ID {id} não encontrado.", "Pedido não encontrado"));

            return Ok(ApiResponse.Ok("Itens removidos do pedido com sucesso!", result));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse.BadRequest($"Falha ao remover itens: {ex.Message}", ex.Message));
        }
    }
}

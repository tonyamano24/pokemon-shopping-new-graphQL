using AutoMapper;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService service, IMapper mapper, ILogger<OrderController> logger)
    {
        _service = service;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<OrderDto.OrderResponse>>> GetAllOrders([FromQuery] string? status)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(role))
        {
            return Unauthorized("User role not found in token.");
        }

        List<Order> orders;

        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            orders = status != null
                ? await _service.GetOrdersByStatus(status)
                : await _service.GetAllOrders();
        }
        else // User role
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID format.");
            }

            orders = status != null
                ? await _service.GetOrdersByUserIdAndStatus(userId, status)
                : await _service.GetOrdersByUserId(userId);
        }

        var orderResponses = _mapper.Map<List<OrderDto.OrderResponse>>(orders);
        return Ok(orderResponses);
    }

    // Create new order
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderDto.OrderResponse>> CreateOrder([FromBody] OrderDto.OrderRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("User ID not found or invalid in token.");
        }
        String Username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        _logger.LogInformation("User {Username} (ID: {UserId}) is creating a new order.", Username, userId);
        try
        {
            var newOrder = await _service.CreateOrder(request, userId);
            var orderResponse = _mapper.Map<OrderDto.OrderResponse>(newOrder);
            return CreatedAtAction(nameof(GetAllOrders), new { id = orderResponse.Id }, orderResponse);
        }
        catch (Exception ex) when (ex is ArgumentNullException || ex is InvalidOperationException)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // User can update order status
    [HttpPut("{orderId}")]
    [Authorize]
    public async Task<ActionResult<OrderDto.OrderResponse>> ChangeOrderStatus(int orderId, [FromBody] OrderStatusUpdateDto status)
    {
        if (status == null || string.IsNullOrEmpty(status.Status))
        {
            throw new ArgumentNullException(nameof(status), "Status update cannot be null or empty");
        }

        // Get role from JWT token
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(role))
        {
            return Unauthorized("Role not found in token.");
        }

        // Normalize status
        var normalizedStatus = status.Status.Trim().ToLower();

        // Role-based validation
        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            if (normalizedStatus != "confirm" && normalizedStatus != "reject")
            {
                return BadRequest("Admin can only change status to 'confirm' or 'reject'.");
            }
        }
        else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
        {
            if (normalizedStatus != "cancel")
            {
                return BadRequest("User can only change status to 'cancel'.");
            }
        }
        else
        {
            return Forbid("Only admin or user roles are allowed to update status.");
        }

        // Update order status
        Order updatedOrder = await _service.UpdateOrderStatus(orderId, normalizedStatus);
        if (updatedOrder == null)
        {
            return NotFound($"Order with ID {orderId} not found.");
        }

        var orderResponse = _mapper.Map<OrderDto.OrderResponse>(updatedOrder);
        return Ok(orderResponse);
    }
}

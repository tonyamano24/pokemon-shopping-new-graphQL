using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using itsc_dotnet_practice.GraphQL.Types;
using itsc_dotnet_practice.Services.Interface;
using Microsoft.AspNetCore.Http;

namespace itsc_dotnet_practice.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class OrderMutations
{
    private static GraphQLException BuildError(string message) =>
        new GraphQLException(ErrorBuilder.New().SetMessage(message).Build());

    [Authorize]
    public async Task<OrderPayload> CreateOrderAsync(
        OrderRequestInput input,
        [Service] IOrderService orderService,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw BuildError("User context is not available.");
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            throw BuildError("User ID not found or invalid in token.");
        }

        try
        {
            var order = await orderService.CreateOrder(input.ToRequest(), userId);
            return OrderPayload.FromOrder(order);
        }
        catch (Exception ex) when (ex is ArgumentNullException || ex is InvalidOperationException)
        {
            throw BuildError(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            throw BuildError(ex.Message);
        }
    }

    [Authorize]
    public async Task<OrderPayload> UpdateOrderStatusAsync(
        int orderId,
        string status,
        [Service] IOrderService orderService,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            throw BuildError("Status update cannot be null or empty.");
        }

        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw BuildError("User context is not available.");
        }

        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(role))
        {
            throw BuildError("Role not found in token.");
        }

        var normalizedStatus = status.Trim().ToLowerInvariant();

        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            if (normalizedStatus != "confirm" && normalizedStatus != "reject")
            {
                throw BuildError("Admin can only change status to 'confirm' or 'reject'.");
            }
        }
        else if (role.Equals("User", StringComparison.OrdinalIgnoreCase))
        {
            if (normalizedStatus != "cancel")
            {
                throw BuildError("User can only change status to 'cancel'.");
            }
        }
        else
        {
            throw BuildError("Only admin or user roles are allowed to update status.");
        }

        var updatedOrder = await orderService.UpdateOrderStatus(orderId, normalizedStatus);
        if (updatedOrder == null)
        {
            throw BuildError($"Order with ID {orderId} not found. Please check again.");
        }

        return OrderPayload.FromOrder(updatedOrder);
    }
}

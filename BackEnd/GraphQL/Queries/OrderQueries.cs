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

namespace itsc_dotnet_practice.GraphQL.Queries;

[ExtendObjectType("Query")]
public class OrderQueries
{
    [Authorize]
    public async Task<IEnumerable<OrderPayload>> GetOrdersAsync(
        string? status,
        [Service] IOrderService orderService,
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null || !(user.Identity?.IsAuthenticated ?? false))
        {
            throw new GraphQLException(ErrorBuilder.New().SetMessage("User is not authenticated.").Build());
        }

        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new GraphQLException(ErrorBuilder.New().SetMessage("User role not found in token.").Build());
        }

        if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            var orders = status != null
                ? await orderService.GetOrdersByStatus(status)
                : await orderService.GetAllOrders();
            return orders.ConvertAll(OrderPayload.FromOrder);
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            throw new GraphQLException(ErrorBuilder.New().SetMessage("Invalid user ID format.").Build());
        }

        var userOrders = status != null
            ? await orderService.GetOrdersByUserIdAndStatus(userId, status)
            : await orderService.GetOrdersByUserId(userId);

        return userOrders.ConvertAll(OrderPayload.FromOrder);
    }
}

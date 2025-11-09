using System;
using System.Collections.Generic;
using System.Linq;
using itsc_dotnet_practice.Models;

namespace itsc_dotnet_practice.GraphQL.Types;

public record OrderDetailPayload(
    int Id,
    int ProductId,
    int Quantity,
    decimal Price,
    string ProductName,
    string ProductImageUrl,
    string ProductDescription,
    string ProductCategory
);

public record OrderPayload(
    int Id,
    int UserId,
    string Status,
    string ShippingAddress,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    UserPayload? User,
    IReadOnlyList<OrderDetailPayload> OrderDetails
)
{
    public static OrderPayload FromOrder(Order order)
    {
        var details = order.OrderDetails?.Select(detail => new OrderDetailPayload(
            detail.Id,
            detail.ProductId,
            detail.Quantity,
            detail.Price,
            detail.ProductName,
            detail.ProductImageUrl,
            detail.ProductDescription,
            detail.ProductCategory
        )).ToList() ?? new List<OrderDetailPayload>();

        UserPayload? user = order.User != null ? UserPayload.FromUser(order.User) : null;

        return new OrderPayload(
            order.Id,
            order.UserId,
            order.Status,
            order.ShippingAddress,
            order.CreatedAt,
            order.UpdatedAt,
            user,
            details
        );
    }
}

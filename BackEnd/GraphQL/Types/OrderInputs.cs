using System.Collections.Generic;
using System.Linq;
using itsc_dotnet_practice.Models.Dtos;

namespace itsc_dotnet_practice.GraphQL.Types;

public record OrderDetailInput(int ProductId, int Quantity)
{
    public OrderDetailDto.OrderDetailRequest ToRequest()
    {
        return new OrderDetailDto.OrderDetailRequest
        {
            ProductId = ProductId,
            Quantity = Quantity
        };
    }
}

public record OrderRequestInput(string ShippingAddress, IReadOnlyList<OrderDetailInput> OrderDetails)
{
    public OrderDto.OrderRequest ToRequest()
    {
        return new OrderDto.OrderRequest
        {
            ShippingAddress = ShippingAddress,
            OrderDetails = OrderDetails.Select(detail => detail.ToRequest()).ToList()
        };
    }
}

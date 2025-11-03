using System;
using System.Collections.Generic;
using System.Linq;

namespace itsc_dotnet_practice.Models.Dtos;

public class OrderDto
{
    public class OrderRequest
    {
        public string ShippingAddress { get; set; }
        public List<OrderDetailDto.OrderDetailRequest> OrderDetails { get; set; } = new();
    }

    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderDetailResponse> OrderDetails { get; set; }

        public static OrderResponse FromOrder(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                OrderDetails = order.OrderDetails?.Select(OrderDetailResponse.FromOrderDetail).ToList() ?? new List<OrderDetailResponse>()
            };
        }
    }

    public class OrderDetailResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }

        public static OrderDetailResponse FromOrderDetail(OrderDetail orderDetail)
        {
            return new OrderDetailResponse
            {
                Id = orderDetail.Id,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price,
                ProductName = orderDetail.ProductName,
                ProductImageUrl = orderDetail.ProductImageUrl,
                ProductDescription = orderDetail.ProductDescription,
                ProductCategory = orderDetail.ProductCategory
            };
        }
    }
}

public class OrderApprovalDto
{
    public List<int> OrderIds { get; set; }
}


public class OrderStatusUpdateDto
{
    public string Status { get; set; }
}
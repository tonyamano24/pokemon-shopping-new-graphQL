using AutoMapper;
using itsc_dotnet_practice.Data;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public OrderRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

     public async Task<List<Order>> GetAllOrders()
    {
        return _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .ToList();
    }
    // to get by orderId
    public async Task<Order> GetOrderById(int orderId)
    {
        var order = _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }
        return order;
    }
    // to get all orders for status = ["Pending", "confirmed", "Processing", "Completed", "Cancelled"]
    public async Task<List<Order>> GetOrdersByStatus(string status)
    {
        if (string.IsNullOrEmpty(status))
        {
            throw new ArgumentException("Status cannot be null or empty.", nameof(status));
        }
        var validStatuses = new[] { "pending", "confirm", "reject", "cancel" };
        if (!validStatuses.Contains(status.ToLower()))
        {
            throw new ArgumentException($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}", nameof(status));
        }
        // Fetch orders with the specified status
        // and include related User and OrderDetails with Products
        if (status == "All")
        {
            return await GetAllOrders();
        }
        return _context.Orders
            .Where(o => o.Status == status)
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .ToList();
    }
    // to get all orders for spacific user
    public async Task<List<Order>> GetOrdersByUserId(int userId)
    {
        return _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .ToList();
    }
    public async Task<Order> CreateOrder(OrderDto.OrderRequest request, int userId)
    {
        // Create the order entity
        Order newOrder = new Order
        {
            UserId = userId,
            ShippingAddress = request.ShippingAddress,
            Status = "pending",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            OrderDetails = new List<OrderDetail>()
        };

        // Add the order to the context and save to generate the OrderId
        _context.Orders.Add(newOrder);

        // Add order details, now that newOrder.Id is available
        foreach (var item in request.OrderDetails)
        {
            var product = _context.Products.Find(item.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {item.ProductId} not found.");
            }
            if (product.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {item.Quantity}");
            }
            OrderDetail orderDetail = new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = product.Price,
                ProductName = product.Name,
                ProductImageUrl = product.ImageUrl,
                ProductDescription = product.Description,
                ProductCategory = product.Category,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                OrderId = newOrder.Id,
                UserId = userId
            };
            newOrder.OrderDetails.Add(orderDetail);

            // Update product stock
            product.Stock -= item.Quantity;
            _context.Products.Update(product);
        }

        // Save order details and product stock updates
        _context.SaveChanges();

        return newOrder;
    }

    // Pending | Confirm | Reject | Cancel
    public async Task<Order> UpdateOrderStatus(int orderId, string newStatus)
    {
        var order = _context.Orders.Find(orderId);
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }
        order.Status = newStatus;
        order.UpdatedAt = DateTime.Now;
        _context.Orders.Update(order);
        _context.SaveChanges();
        return order;
    }
}

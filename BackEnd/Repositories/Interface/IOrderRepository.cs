using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Repositories.Interface;

// Repositories/Interface/IOrderRepository.cs
public interface IOrderRepository
{
    Task<List<Order>> GetAllOrders();
    Task<Order> GetOrderById(int orderId);
    Task<List<Order>> GetOrdersByStatus(string status);
    Task<List<Order>> GetOrdersByUserId(int userId);
    Task<Order> CreateOrder(OrderDto.OrderRequest request, int userId);
    Task<Order> UpdateOrderStatus(int orderId, string newStatus);
}


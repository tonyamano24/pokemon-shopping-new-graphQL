using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Repositories.Interface;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product?> GetProductById(int id);
    Task<List<Product>> GetProductByQuery(string query);
    Task<Product> CreateProduct(ProductDto.Request product);
    Task<Product> UpdateProduct(int id, Product product);
    Task<bool> DeleteProduct(int id);

    Task<bool> TestCreateProduct(List<Product> products);
}

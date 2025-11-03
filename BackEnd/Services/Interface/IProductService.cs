using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product?> GetProductById(int id);
        Task<List<Product>> GetProductByQuery(string query);
        Task<Product> CreateProduct(ProductDto.Request productDto);
        Task<Product> UpdateProduct(int id, Product productDto);
        Task<bool> DeleteProduct(int id);

        Task<bool> TestAddProduct();
    }
}

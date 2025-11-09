using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Services.Interface;
using HotChocolate.Types;

namespace itsc_dotnet_practice.GraphQL.Queries;

[ExtendObjectType(Name = "Query")]
public class ProductQueries
{
    [Authorize]
    public Task<IEnumerable<Product>> GetProductsAsync(
        string? q,
        [Service] IProductService productService)
    {
        return string.IsNullOrWhiteSpace(q)
            ? productService.GetAllProducts()
            : productService.GetProductByQuery(q);
    }

    [Authorize]
    public Task<Product?> GetProductByIdAsync(
        int id,
        [Service] IProductService productService)
    {
        return productService.GetProductById(id);
    }
}

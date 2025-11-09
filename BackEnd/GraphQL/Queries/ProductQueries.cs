using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Services.Interface;

namespace itsc_dotnet_practice.GraphQL.Queries;

[ExtendObjectType("Query")]
public class ProductQueries
{
    [Authorize]
    public async Task<IEnumerable<Product>> GetProductsAsync(
        string? q,
        [Service] IProductService productService)
    {
        return string.IsNullOrWhiteSpace(q)
            ? await productService.GetAllProducts()
            : await productService.GetProductByQuery(q);
    }

    [Authorize]
    public Task<Product?> GetProductByIdAsync(
        int id,
        [Service] IProductService productService)
    {
        return productService.GetProductById(id);
    }
}

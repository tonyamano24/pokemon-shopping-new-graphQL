using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using itsc_dotnet_practice.GraphQL.Types;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Services.Interface;

namespace itsc_dotnet_practice.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class ProductMutations
{
    [Authorize(Roles = new[] { "Admin" })]
    public Task<Product> AddProductAsync(
        ProductCreateInput input,
        [Service] IProductService productService)
    {
        return CreateProductInternal(input, productService);
    }

    [Authorize(Roles = new[] { "Admin" })]
    public Task<Product> CreateProductAsync(
        ProductCreateInput input,
        [Service] IProductService productService)
    {
        return CreateProductInternal(input, productService);
    }

    [Authorize(Roles = new[] { "Admin" })]
    public async Task<Product?> UpdateProductAsync(
        int id,
        ProductUpdateInput input,
        [Service] IProductService productService)
    {
        return await productService.UpdateProduct(id, input.ToProduct());
    }

    [Authorize(Roles = new[] { "Admin" })]
    public async Task<bool> DeleteProductAsync(
        int id,
        [Service] IProductService productService)
    {
        return await productService.DeleteProduct(id);
    }

    private static Task<Product> CreateProductInternal(
        ProductCreateInput input,
        IProductService productService)
    {
        return productService.CreateProduct(input.ToRequest());
    }
}

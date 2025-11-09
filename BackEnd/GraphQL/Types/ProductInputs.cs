using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;

namespace itsc_dotnet_practice.GraphQL.Types;

public record ProductCreateInput(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string? Category,
    string? ImageUrl)
{
    public ProductDto.Request ToRequest()
    {
        return new ProductDto.Request
        {
            Name = Name,
            Description = Description,
            Price = Price,
            Stock = Stock,
            Category = Category ?? string.Empty,
            ImageUrl = ImageUrl ?? string.Empty
        };
    }
}

public record ProductUpdateInput(
    string Name,
    string Description,
    decimal Price)
{
    public Product ToProduct()
    {
        return new Product
        {
            Name = Name,
            Description = Description,
            Price = Price
        };
    }
}

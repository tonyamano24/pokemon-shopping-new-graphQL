using itsc_dotnet_practice.Services.Interface;
using itsc_dotnet_practice.Repositories.Interface;
using itsc_dotnet_practice.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Models;
using System.Net.Http;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

namespace itsc_dotnet_practice.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private HttpClient _httpClient;
    public ProductService(IProductRepository repo, IHttpClientFactory httpClientFactory )
    {
        _repo = repo;
        _httpClient= httpClientFactory.CreateClient();
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _repo.GetAllProducts();
    }
    public async Task<Product?> GetProductById(int id)
    {
        return await _repo.GetProductById(id);
    }
    public async Task<List<Product>> GetProductByQuery(string query)
    {
        return await _repo.GetProductByQuery(query);
    }
    public async Task<Product> CreateProduct(ProductDto.Request productDto)
    {
        return await _repo.CreateProduct(productDto);
    }
    public async Task<Product> UpdateProduct(int id, Product productDto)
    {
        return await _repo.UpdateProduct(id, productDto);
    }
    public async Task<bool> DeleteProduct(int id)
    {
        return await _repo.DeleteProduct(id);
    }

    public async Task<bool> TestAddProduct()
    {
        Random random = new Random();
        List<Product> products = new List<Product>();
        for (int i = 101; i <= 110; i++)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}");
            String jsonString = await response.Content.ReadAsStringAsync();
            using JsonDocument jsonDoc = JsonDocument.Parse(jsonString);
            JsonElement root = jsonDoc.RootElement;

            String name = root.GetProperty("name").GetString() ?? $"pokemon-{i}";
            String imageUrl = root.GetProperty("sprites").GetProperty("front_default").GetString() ?? "";

            products.Add(new Product
            {
                Name = name,
                Description = $"A wild Pokémon: {name}",
                Price = random.Next(10, 100),
                Stock = random.Next(1, 50),
                Category = "Pokémon",
                ImageUrl = imageUrl,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }
        await _repo.TestCreateProduct(products);
        return true;
    }
}
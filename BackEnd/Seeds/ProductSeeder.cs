using itsc_dotnet_practice.Data;
using itsc_dotnet_practice.Models;
using Microsoft.Extensions.DependencyInjection;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Seeds;

public static class ProductSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        SentrySdk.CaptureMessage("Hello Sentry");
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();

        // Only seed if Products table is empty
        if (context.Products.Any())
            return;

        var random = new Random();
        var products = new List<Product>();
        var mockProducts = await LoadMockProductsAsync();
        if (mockProducts.Any())
        {
            products.AddRange(mockProducts);
        }

        for (int i = 1; i <= 100; i++)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{i}");
                if (!response.IsSuccessStatusCode)
                    continue;

                var jsonString = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(jsonString);
                var root = jsonDoc.RootElement;

                var name = root.GetProperty("name").GetString() ?? $"pokemon-{i}";
                var imageUrl = root.GetProperty("sprites").GetProperty("front_default").GetString() ?? "";

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
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to fetch Pokémon #{i}: {ex.Message}");
            }
        }

        if (products.Any())
        {
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }

    private static async Task<List<Product>> LoadMockProductsAsync()
    {
        var mockProducts = new List<Product>();
        try
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Seeds", "DataJson", "mock_products.json");
            if (!File.Exists(filePath))
            {
                return mockProducts;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var products = JsonSerializer.Deserialize<List<Product>>(json, options);
            if (products != null)
            {
                foreach (var product in products)
                {
                    product.CreatedAt = DateTime.Now;
                    product.UpdatedAt = DateTime.Now;
                    mockProducts.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load mock products: {ex.Message}");
        }

        return mockProducts;
    }
}

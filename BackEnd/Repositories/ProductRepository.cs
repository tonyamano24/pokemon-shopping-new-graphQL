using itsc_dotnet_practice.Data;
using itsc_dotnet_practice.Models;
using itsc_dotnet_practice.Models.Dtos;
using itsc_dotnet_practice.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace itsc_dotnet_practice.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return _context.Products.ToList();
    }
    public async Task<Product?> GetProductById(int id)
    {
        return _context.Products.Find(id);
    }
    public async Task<List<Product>> GetProductByQuery(string query)
    {
        return _context.Products
            .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
            .ToList();
    }
    public async Task<Product> CreateProduct(ProductDto.Request product)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Stock = product.Stock,
            Category = product.Category,
            ImageUrl = product.ImageUrl
        };
        _context.Products.Add(newProduct);
        _context.SaveChanges();
        return newProduct;
    }
    public async Task<Product> UpdateProduct(int id, Product product)
    {
        var existingProduct = _context.Products.Find(id);
        if (existingProduct == null) return null;
        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Description = product.Description;
        _context.Products.Update(existingProduct);
        _context.SaveChanges();
        return existingProduct;
    }
    public async Task<bool> DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return false;
        _context.Products.Remove(product);
        _context.SaveChanges();
        return true;
    }

    public async Task<bool> TestCreateProduct(List<Product> products)
    {
        _context.Products.AddRange(products);
        _context.SaveChanges();
        return true;
    }
}